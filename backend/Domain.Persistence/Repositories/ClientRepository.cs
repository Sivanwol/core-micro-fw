using Application.Exceptions;
using Application.Utils;
using Domain.DTO.Client;
using Domain.Entities;
using Domain.Filters;
using Domain.Persistence.Context;
using Domain.Persistence.Repositories.Common;
using Domain.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Domain.Persistence.Repositories;

public class ClientRepository : BaseRepository, IClientRepository
{
    public ClientRepository(
        IDomainContext context
    ) : base(context) { }
    public async Task<int> Add(CreateClient client)
    {
        var clientRecord = new Clients
        {
            OwnerUserId = client.OwnerUserId,
            Parent = null,
            Name = client.Name,
            Description = client.Description,
            Website = client.Website,
            CountryId = client.CountryId,
            Address = client.Address,
            City = client.City,

        };
        Context.Clients.Add(clientRecord);
        await Context.Instance.SaveChangesAsync();
        return clientRecord.Id;
    }
    public async Task<int> Add(int parentId, CreateClient client)
    {
        var clientRecord = new Clients
        {
            OwnerUserId = client.OwnerUserId,
            ParentId = parentId,
            Name = client.Name,
            Description = client.Description,
            Website = client.Website,
            CountryId = client.CountryId,
            Address = client.Address,
            City = client.City,

        };
        Context.Clients.Add(clientRecord);
        await Context.Instance.SaveChangesAsync();
        return clientRecord.Id;
    }

    public async Task Update(int clientId, CreateClient client)
    {
        var clientRecord = await Context.Clients.FirstOrDefaultAsync(w => w.Id == clientId);
        if (clientRecord == null)
        {
            throw new Exception("Client not found");
        }
        clientRecord.OwnerUserId = client.OwnerUserId;
        clientRecord.Name = client.Name;
        clientRecord.Description = client.Description;
        clientRecord.Website = client.Website;
        clientRecord.CountryId = client.CountryId;
        clientRecord.Address = client.Address;
        clientRecord.City = client.City;
        await Context.Instance.SaveChangesAsync();
    }

    public async Task Update(int clientId, int parentId, CreateClient client)
    {
        var clientRecord = await Context.Clients.FirstOrDefaultAsync(w => w.Id == clientId);
        if (clientRecord == null)
        {
            throw new Exception("Client not found");
        }
        clientRecord.OwnerUserId = client.OwnerUserId;
        clientRecord.ParentId = parentId;
        clientRecord.Name = client.Name;
        clientRecord.Description = client.Description;
        clientRecord.Website = client.Website;
        clientRecord.CountryId = client.CountryId;
        clientRecord.Address = client.Address;
        clientRecord.City = client.City;
        await Context.Instance.SaveChangesAsync();
    }
    public async Task Delete(int clientId)
    {
        var clientRecord = await Context.Clients.FirstOrDefaultAsync(w => w.Id == clientId);
        if (clientRecord == null)
        {
            throw new EntityNotFoundException(nameof(Clients), clientId.ToString());
        }
        clientRecord.DeletedAt = SystemClock.Now();
        Context.Instance.Update(clientRecord);
        await Context.Instance.SaveChangesAsync();
        await DeleteAllContacts(clientId);
    }
    public async Task Restore(int clientId)
    {
        var clientRecord = Context.Clients.FirstOrDefault(w => w.Id == clientId && w.DeletedAt != null);
        if (clientRecord == null)
        {
            throw new EntityNotFoundException(nameof(Clients), clientId.ToString());
        }
        clientRecord.DeletedAt = null;
        Context.Instance.Update(clientRecord);
        await Context.Instance.SaveChangesAsync();
    }
    public async Task<bool> IsClientExists(int clientId)
    {
        var client = await Context.Clients.FirstOrDefaultAsync(w => w.Id == clientId);
        return client != null;
    }
    public async Task<ICollection<Clients>> GetAll(RecordFilterPagination<ClientFilters> filter)
    {
        var query = Context.Clients.AsQueryable();

        query = query.Include(i => i.Country);
        query = query.Include(i => i.OwnerUser);
        query = filter.ApplyQuery(query);
        query = query.Where(w => w.DeletedAt == null);
        return await query.ToListAsync();
    }
    public async Task<ICollection<Clients>> GetAll(RecordFilterPagination<ClientFilters> filter, bool onlyDeleted)
    {
        var query = Context.Clients.AsQueryable();
        query = filter.ApplyQuery(query);
        if (onlyDeleted)
        {
            query = query.Where(w => w.DeletedAt != null);
        }
        query = query.Include(i => i.Country);
        query = query.Include(i => i.OwnerUser);

        return await query.ToListAsync();
    }
    public async Task<int> GetAllTotalPages(RecordFilterPagination<ClientFilters> filter)
    {
        var query = Context.Clients.AsQueryable();
        var entitiesCount = await filter.ApplyQueryWithoutPagination(query).Where(w => w.DeletedAt == null).CountAsync();
        return (int)Math.Ceiling((double)entitiesCount / filter.PageSize);
    }
    public async Task<int> GetAllTotalPages(RecordFilterPagination<ClientFilters> filter, bool onlyDeleted)
    {
        var query = Context.Clients.AsQueryable();
        query = filter.ApplyQueryWithoutPagination(query);
        if (onlyDeleted)
        {
            query = query.Where(w => w.DeletedAt != null);
        }
        var entitiesCount = await query.CountAsync();
        return (int)Math.Ceiling((double)entitiesCount / filter.PageSize);
    }
    public async Task<int> GetAllCount(RecordFilterPagination<ClientFilters> filter)
    {
        var query = Context.Clients.AsQueryable();
        var entitiesCount = await filter.ApplyQueryWithoutPagination(query).Where(w => w.DeletedAt == null).CountAsync();
        return entitiesCount;
    }
    public async Task<int> GetAllCount(RecordFilterPagination<ClientFilters> filter, bool onlyDeleted)
    {
        var query = Context.Clients.AsQueryable();
        query = filter.ApplyQueryWithoutPagination(query);
        if (onlyDeleted)
        {
            query = query.Where(w => w.DeletedAt != null);
        }
        var entitiesCount = await query.CountAsync();
        return entitiesCount;
    }
    public async Task<int> AddContact(int clientId, CreateClientContact clientContact)
    {
        var client = await Context.Clients.FirstOrDefaultAsync(w => w.Id == clientId && w.DeletedAt == null);
        if (client == null)
        {
            throw new EntityNotFoundException(nameof(Clients), clientId.ToString());
        }
        var clientContactRecord = new ClientContacts
        {
            Client = client,
            FirstName = clientContact.FirstName,
            LastName = clientContact.LastName,
            Email = clientContact.Email,
            Phone1 = clientContact.Phone1,
            Phone2 = clientContact.Phone2,
            Fax = clientContact.Fax,
            Whatsapp = clientContact.Whatsapp,
            Address = clientContact.Address,
            City = clientContact.City,
            CountryId = clientContact.CountryId,
            State = clientContact.State,
            JobTitle = clientContact.JobTitle,
            Department = clientContact.Department,
            Company = clientContact.Company,
            PostalCode = clientContact.PostalCode,
            Notes = clientContact.Notes,
        };
        Context.ClientContacts.Add(clientContactRecord);
        await Context.Instance.SaveChangesAsync();
        return clientContactRecord.Id;
    }
    public async Task UpdateContact(int clientId, int clientContactId, UpdateClientContact clientContact)
    {
        var clientContactRecord = await Context.ClientContacts.FirstOrDefaultAsync(w => w.Id == clientContactId && w.ClientId == clientId && w.DeletedAt == null);
        if (clientContactRecord == null)
        {
            throw new Exception("Client contact not found");
        }

        if (clientContact.FirstName != null)
            clientContactRecord.FirstName = clientContact.FirstName;
        if (clientContact.LastName != null)
            clientContactRecord.LastName = clientContact.LastName;
        if (clientContact.Email != null)
            clientContactRecord.Email = clientContact.Email;
        if (clientContact.Phone1 != null)
            clientContactRecord.Phone1 = clientContact.Phone1;
        if (clientContact.Phone2 != null)
            clientContactRecord.Phone2 = clientContact.Phone2;
        if (clientContact.Fax != null)
            clientContactRecord.Fax = clientContact.Fax;
        if (clientContact.Whatsapp != null)
            clientContactRecord.Whatsapp = clientContact.Whatsapp;
        if (clientContact.Address != null)
            clientContactRecord.Address = clientContact.Address;
        if (clientContact.City != null)
            clientContactRecord.City = clientContact.City;
        if (clientContact.CountryId != null)
            clientContactRecord.CountryId = clientContact.CountryId.Value;
        if (clientContact.State != null)
            clientContactRecord.State = clientContact.State;
        if (clientContact.JobTitle != null)
            clientContactRecord.JobTitle = clientContact.JobTitle;
        if (clientContact.Department != null)
            clientContactRecord.Department = clientContact.Department;
        if (clientContact.Company != null)
            clientContactRecord.Company = clientContact.Company;
        if (clientContact.PostalCode != null)
            clientContactRecord.PostalCode = clientContact.PostalCode;
        if (clientContact.Notes != null)
            clientContactRecord.Notes = clientContact.Notes;
        await Context.Instance.SaveChangesAsync();
    }
    public async Task DeleteContact(int clientId, int clientContactId)
    {
        var clientContactRecord = Context.ClientContacts.FirstOrDefault(w => w.Id == clientContactId && w.ClientId == clientId && w.DeletedAt == null);
        if (clientContactRecord == null)
        {
            throw new EntityNotFoundException(nameof(ClientContacts), clientContactId.ToString());
        }
        clientContactRecord.DeletedAt = SystemClock.Now();
        Context.Instance.Update(clientContactRecord);
        await Context.Instance.SaveChangesAsync();
    }
    public async Task RestoreContact(int clientId, int clientContactId)
    {
        var clientContactRecord = Context.ClientContacts.FirstOrDefault(w => w.Id == clientContactId && w.ClientId == clientId && w.DeletedAt != null);
        if (clientContactRecord == null)
        {
            throw new EntityNotFoundException(nameof(ClientContacts), clientContactId.ToString());
        }
        clientContactRecord.DeletedAt = null;
        Context.Instance.Update(clientContactRecord);
        await Context.Instance.SaveChangesAsync();
    }

    public Task DeleteAllContacts(int clientId)
    {
        var clientContactRecord = Context.ClientContacts.Where(w => w.ClientId == clientId);
        foreach (var clientContact in clientContactRecord)
        {
            clientContact.DeletedAt = DateTime.UtcNow;
        }
        Context.Instance.UpdateRange(clientContactRecord);
        return Context.Instance.SaveChangesAsync();
    }

    public async Task RestoreAllContacts(int clientId)
    {
        var clientContactRecord = Context.ClientContacts.Where(w => w.ClientId == clientId && w.DeletedAt != null);
        foreach (var clientContact in clientContactRecord)
        {
            clientContact.DeletedAt = null;
        }
        Context.Instance.UpdateRange(clientContactRecord);
        await Context.Instance.SaveChangesAsync();
    }

    public async Task ClearClientSoftDeleted()
    {
        // fetch all soft deleted clients from two weeks  and delete them
        var clients = Context.Clients.Where(w => w.DeletedAt != null && w.DeletedAt <= DateTime.UtcNow.AddDays(-14));
        // TODO - add soft delete for client contacts and the rest of the data that is related to this client
        Context.Instance.RemoveRange(clients);
        await Context.Instance.SaveChangesAsync();
    }

    public async Task ClearClientContactSoftDeleted()
    {
        var clientContacts = Context.ClientContacts.Where(w => w.DeletedAt != null && w.DeletedAt <= DateTime.UtcNow.AddDays(-14));
        Context.Instance.RemoveRange(clientContacts);
        await Context.Instance.SaveChangesAsync();
    }

    public async Task<ICollection<ClientContacts>> GetClientContactsAll(int clientId, RecordFilterPagination<ClientContactsFilters> filter)
    {
        var query = Context.ClientContacts.AsQueryable();
        query = query.Where(w => w.DeletedAt == null && w.ClientId == clientId);
        query = query.Include(i => i.Country);
        query = filter.ApplyQuery(query);
        return await query.ToListAsync();
    }
    public async Task<ICollection<ClientContacts>> GetClientContactsAll(int clientId, RecordFilterPagination<ClientContactsFilters> filter, bool onlyDeleted)
    {
        var query = Context.ClientContacts.AsQueryable();
        query = filter.ApplyQuery(query);
        query = query.Where(w => w.ClientId == clientId);
        if (onlyDeleted)
        {
            query = query.Where(w => w.DeletedAt != null);
        }
        return await query.ToListAsync();
    }
    public async Task<int> GetClientContactsAllTotalPages(int clientId, RecordFilterPagination<ClientContactsFilters> filter)
    {
        var query = Context.ClientContacts.AsQueryable();
        query = filter.ApplyQuery(query);
        query = query.Where(w => w.DeletedAt == null && w.ClientId == clientId);
        return (int)Math.Ceiling((double)await query.CountAsync() / filter.PageSize);
    }
    public async Task<int> GetClientContactsAllTotalPages(int clientId, RecordFilterPagination<ClientContactsFilters> filter, bool onlyDeleted)
    {
        var query = Context.ClientContacts.AsQueryable();
        query = filter.ApplyQuery(query);
        query = query.Where(w => w.ClientId == clientId);
        if (onlyDeleted)
        {
            query = query.Where(w => w.DeletedAt != null);
        }

        return await query.CountAsync();
    }
    public async Task<int> GetClientContactsAllCount(int clientId, RecordFilterPagination<ClientContactsFilters> filter)
    {
        var query = Context.ClientContacts.AsQueryable();
        query = query.Where(w => w.DeletedAt == null && w.ClientId == clientId);
        query = filter.ApplyQuery(query);
        return await query.CountAsync();
    }
    public async Task<int> GetClientContactsAllCount(int clientId, RecordFilterPagination<ClientContactsFilters> filter, bool onlyDeleted)
    {
        var query = Context.ClientContacts.AsQueryable();
        query = filter.ApplyQuery(query);
        query = query.Where(w => w.ClientId == clientId);
        if (onlyDeleted)
        {
            query = query.Where(w => w.DeletedAt != null);
        }
        return await query.CountAsync();

    }
    public async Task<Clients?> GetById(int id)
    {
        var client = await Context.Clients.FirstOrDefaultAsync(w => w.Id == id);
        return client;
    }
    public async Task<ClientContacts?> GetClientContactById(int clientId, int clientContactId)
    {
        var clientContact = await Context.ClientContacts.FirstOrDefaultAsync(w => w.Id == clientContactId && w.DeletedAt == null && w.ClientId == clientId);
        return clientContact;
    }
    public async Task<Clients?> GetByIdWithChildren(int id)
    {
        var client = await Context.Clients
            .Include(i => i.Children)
            .FirstOrDefaultAsync(w => w.Id == id && w.DeletedAt == null);
        return client;
    }
    public async Task<bool> IsClientContactExists(int clientId, string firstName, string lastName)
    {
        var clientContact =
            await Context.ClientContacts.FirstOrDefaultAsync(w => w.ClientId == clientId && w.DeletedAt == null && w.FirstName == firstName && w.LastName == lastName);
        return clientContact != null;
    }

    public async Task<bool> HasClientExist(string clientName, int countryId)
    {
        return await Context.Clients.AnyAsync(w => w.Name == clientName && w.CountryId == countryId && w.DeletedAt == null);
    }
}