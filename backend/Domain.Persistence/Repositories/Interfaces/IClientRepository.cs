using Domain.DTO.Client;
using Domain.Entities;
using Domain.Filters;
using Domain.Persistence.Repositories.Common.Interfaces;
namespace Domain.Persistence.Repositories.Interfaces;

public interface IClientRepository : IGenericRepository<Clients, int> {
    Task<int> Add(CreateClient client);
    Task<int> Add(int parentId, CreateClient client);
    Task Update(int clientId, CreateClient client);
    Task Update(int clientId, int parentId, CreateClient client);
    Task Delete(int clientId);
    Task Restore(int clientId);

    Task<bool> IsClientExists(int clientId);

    Task<ICollection<Clients>> GetAll(RecordFilterPagination<ClientFilters> filter);
    Task<ICollection<Clients>> GetAll(RecordFilterPagination<ClientFilters> filter, bool onlyDeleted);
    Task<int> GetAllTotalPages(RecordFilterPagination<ClientFilters> filter);
    Task<int> GetAllTotalPages(RecordFilterPagination<ClientFilters> filter, bool onlyDeleted);
    Task<int> GetAllCount(RecordFilterPagination<ClientFilters> filter);

    Task<int> GetAllCount(RecordFilterPagination<ClientFilters> filter, bool onlyDeleted);

    Task<bool> HasClientExist(string clientName, int countryId);
    Task<int> AddContact(int clientId, CreateClientContact clientContact);
    Task UpdateContact(int clientId, int clientContactId, UpdateClientContact clientContact);
    Task DeleteContact(int clientId, int clientContactId);
    Task RestoreContact(int clientId, int clientContactId);
    Task DeleteAllContacts(int clientId);
    Task RestoreAllContacts(int clientId);

    Task ClearClientSoftDeleted();
    Task ClearClientContactSoftDeleted();
    Task<ICollection<ClientContacts>> GetClientContactsAll(int clientId, RecordFilterPagination<ClientContactsFilters> filter);
    Task<ICollection<ClientContacts>> GetClientContactsAll(int clientId, RecordFilterPagination<ClientContactsFilters> filter, bool onlyDeleted);
    Task<int> GetClientContactsAllTotalPages(int clientId, RecordFilterPagination<ClientContactsFilters> filter);
    Task<int> GetClientContactsAllTotalPages(int clientId, RecordFilterPagination<ClientContactsFilters> filter, bool onlyDeleted);
    Task<int> GetClientContactsAllCount(int clientId, RecordFilterPagination<ClientContactsFilters> filter);
    Task<int> GetClientContactsAllCount(int clientId, RecordFilterPagination<ClientContactsFilters> filter, bool onlyDeleted);
    Task<ClientContacts?> GetClientContactById(int clientId, int clientContactId);
}