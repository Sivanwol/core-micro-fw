using System.Linq.Dynamic.Core;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Filters;
using Domain.Persistence.Context;
using Domain.Persistence.Repositories.Common;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Requests.Processor.Services.Vendor;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Domain.Persistence.Repositories;

public class VendorRepository : BaseRepository, IVendorRepository
{
    private readonly IMediaRepository _mediaRepository;
    public VendorRepository(IDomainContext context, IMediaRepository mediaRepository) : base(context)
    {
        _mediaRepository = mediaRepository;
    }

    public async Task<Vendors> CreateVendor(CreateVendorRequest request, int logoId)
    {
        Log.Logger.Information($"add vendor [{request.Name} , {request.CountryId}]");
        if (await HasVendorExist(request.UserId, request.Name, request.CountryId))
        {
            throw new EntityFoundException("Vendors", $"[{request.UserId}, {request.Name} , {request.CountryId}]");
        }
        if (!await _mediaRepository.MediaExists(logoId))
        {
            throw new EntityNotFoundException("Media", $"[{logoId}]");
        }
        var vendor = new Vendors
        {
            UserId = request.UserId.ToString(),
            Name = request.Name,
            Description = request.Description,
            CountryId = request.CountryId,
            LogoId = logoId,
            SiteUrl = request.SiteUrl,
            SupportUrl = request.SupportUrl,
            SupportEmail = request.SupportEmail,
            SupportPhone = request.SupportPhone,
            SupportResponseType = request.SupportResponseType,

        };
        await Context.Vendors.AddAsync(vendor);
        await Context.Instance.SaveChangesAsync();
        return vendor;
    }

    public async Task<Vendors> CreateVendor(CreateClientVendorRequest request, int logoId)
    {
        Log.Logger.Information($"add client vendor [{request.Name} , {request.CountryId}]");
        if (await HasVendorExist(request.UserId, request.Name, request.CountryId))
        {
            throw new EntityFoundException("Vendors", $"[{request.UserId}, {request.Name} , {request.CountryId}]");
        }
        if (!await _mediaRepository.MediaExists(logoId))
        {
            throw new EntityNotFoundException("Media", $"[{logoId}]");
        }
        var vendor = new Vendors
        {
            UserId = request.UserId.ToString(),
            ClientId = request.ClientId,
            Name = request.Name,
            Description = request.Description,
            CountryId = request.CountryId,
            LogoId = logoId,
            SiteUrl = request.SiteUrl,
            SupportUrl = request.SupportUrl,
            SupportEmail = request.SupportEmail,
            SupportPhone = request.SupportPhone,
            SupportResponseType = request.SupportResponseType,

        };
        await Context.Vendors.AddAsync(vendor);
        await Context.Instance.SaveChangesAsync();
        return vendor;
    }

    public async Task DeleteVendors(List<int> vendorIds)
    {
        Log.Logger.Information($"delete vendors {vendorIds.ToString()}");
        foreach (var vendorId in vendorIds)
        {
            var vendor = await GetVendor(vendorId);
            if (vendor == null)
            {
                throw new EntityNotFoundException("Vendors", vendorId);
            }
            vendor.DeletedAt = SystemClock.Now();
        }
        await Context.Instance.SaveChangesAsync();
    }

    public async Task<Vendors?> GetVendor(int vendorId)
    {
        Log.Logger.Information($"fetch vendor {vendorId}");
        var query = Context.Vendors
        .Include(x => x.Country)
        .Include(x => x.Contacts)
        .FirstOrDefault(x => x.Id == vendorId && x.DeletedAt == null);
        return query;
    }

    public async Task<IEnumerable<Vendors>> GetVendors(RecordFilterPagination<VendorFilters> filter)
    {
        Log.Logger.Information($"Fetching vendor records with filter {filter}");
        var query = Context.Vendors.AsQueryable();
        query = query.Include(x => x.Country);
        query = query.Include(x => x.Contacts);
        query = query.Where(x => x.DeletedAt == null);
        query = filter.ApplyQuery(query);
        return query.ToList();
    }

    public async Task<int> GetVendorsTotalPages(RecordFilterPagination<VendorFilters> filter)
    {
        Log.Logger.Information($"Fetching vendor total Pages with filter {filter}");
        var query = Context.Vendors.AsQueryable();
        query = query.Include(x => x.Country);
        query = query.Include(x => x.Contacts);
        query = query.Where(x => x.DeletedAt == null);
        return filter.ApplyQuery(query).Count();
    }

    public async Task<int> GetVendorsTotalRecords(RecordFilterPagination<VendorFilters> filter)
    {
        Log.Logger.Information($"Fetching vendor total records with filter {filter}");
        var query = Context.Vendors.AsQueryable();
        query = query.Include(x => x.Country);
        query = query.Include(x => x.Contacts);
        query = query.Where(x => x.DeletedAt == null);
        query = filter.ApplyQuery(query);
        return (int)Math.Ceiling(query.Count() / (double)filter.PageSize);
    }

    public async Task UpdateVendor(UpdateVendorRequest request, int? logoId)
    {
        if (await HasVendorExist(request.UserId, request.Name, request.CountryId))
        {
            throw new EntityFoundException("Vendors", $"[{request.VendorId} ,{request.UserId} ,{request.Name} , {request.CountryId}]");
        }
        var vendor = await GetVendor(request.VendorId);
        if (vendor == null)
        {
            throw new EntityNotFoundException("Vendors", request.VendorId);
        }
        if (vendor.LogoId.HasValue)
        {
            if (!await _mediaRepository.MediaExists(vendor.LogoId!.Value))
            {
                throw new EntityNotFoundException("Media", $"[{vendor.LogoId}]");
            }
        }
        vendor.Name = request.Name;
        vendor.Description = request.Description;
        vendor.Address = request.Address;
        vendor.CountryId = request.CountryId;
        if (logoId != null)
        {
            vendor.LogoId = logoId.Value;
        }
        vendor.SiteUrl = request.SiteUrl;
        vendor.SupportEmail = request.SupportEmail;
        vendor.SupportUrl = request.SupportUrl;
        vendor.SupportPhone = request.SupportPhone;
        vendor.SupportResponseType = request.SupportResponseType;

        await Context.Instance.SaveChangesAsync();
    }

    public async Task UpdateVendor(UpdateClientVendorRequest request, int? logoId)
    {
        if (await HasVendorExist(request.UserId, request.Name, request.CountryId))
        {
            throw new EntityFoundException("Vendors", $"[{request.Name} , {request.CountryId}]");
        }
        var vendor = await GetVendor(request.VendorId);
        if (vendor == null)
        {
            throw new EntityNotFoundException("Vendors", request.VendorId);
        }
        if (vendor.LogoId.HasValue)
        {
            if (!await _mediaRepository.MediaExists(vendor.LogoId!.Value))
            {
                throw new EntityNotFoundException("Media", $"[{vendor.LogoId}]");
            }
        }
        vendor.ClientId = request.ClientId;
        vendor.Name = request.Name;
        vendor.Description = request.Description;
        vendor.Address = request.Address;
        vendor.CountryId = request.CountryId;
        vendor.SiteUrl = request.SiteUrl;
        if (logoId != null)
        {
            vendor.LogoId = logoId.Value;
        }
        vendor.SupportEmail = request.SupportEmail;
        vendor.SupportUrl = request.SupportUrl;
        vendor.SupportPhone = request.SupportPhone;
        vendor.SupportResponseType = request.SupportResponseType;

        await Context.Instance.SaveChangesAsync();
    }

    public async Task<bool> HasVendorExist(Guid userId, string name, int countyId)
    {
        if (string.IsNullOrWhiteSpace(name) || countyId == 0)
        {
            throw new ArgumentNullException();
        }
        var query = await (from w in Context.Vendors
                           where w.CountryId == countyId && w.Name.ToLower().Equals(name.ToLower()) && w.DeletedAt == null
                           select w).FirstOrDefaultAsync();
        return query != null;
    }

    public async Task<bool> HasVendorExist(Guid userId, int clientId, string name, int countyId)
    {
        if (string.IsNullOrWhiteSpace(name) || countyId == 0)
        {
            throw new ArgumentNullException();
        }
        var query = await (from w in Context.Vendors
                           where w.CountryId == countyId && w.Name.ToLower().Equals(name.ToLower()) &&
                           w.DeletedAt == null &&
                           w.ClientId == clientId &&
                           w.UserId == userId.ToString()
                           select w).FirstOrDefaultAsync();
        return query != null;
    }


    public async Task<bool> HasUpdateVendorExist(Guid userId, int vendorId, string name, int countyId)
    {

        if (string.IsNullOrWhiteSpace(name) || countyId == 0)
        {
            throw new ArgumentNullException();
        }
        var query = await (from w in Context.Vendors
                           where w.CountryId == countyId &&
                           w.Name.ToLower().Equals(name.ToLower()) &&
                           w.DeletedAt == null &&
                           w.Id != vendorId &&
                           w.UserId == userId.ToString()
                           select w).FirstOrDefaultAsync();
        return query != null;
    }

    public async Task<bool> HasUpdateVendorExist(Guid userId, int vendorId, int clientId, string name, int countyId)
    {
        if (string.IsNullOrWhiteSpace(name) || countyId == 0)
        {
            throw new ArgumentNullException();
        }
        var query = await (from w in Context.Vendors
                           where w.CountryId == countyId &&
                           w.Name.ToLower().Equals(name.ToLower()) &&
                           w.ClientId == clientId && w.DeletedAt == null &&
                           w.Id != vendorId &&
                           w.UserId == userId.ToString()
                           select w).FirstOrDefaultAsync();
        return query != null;
    }
    public async Task<IEnumerable<Vendors>> GetClientVendors(int clientId, RecordFilterPagination<VendorFilters> filter)
    {
        Log.Logger.Information($"Fetching vendor records with filter {filter}");
        var query = Context.Vendors.AsQueryable();
        query = query.Include(x => x.Country);
        query = query.Include(x => x.Contacts);
        query = query.Where(x => x.ClientId == clientId && x.DeletedAt == null);
        query = filter.ApplyQuery(query);
        return query.ToList();
    }

    public async Task<int> GetClientVendorsTotalRecords(int clientId, RecordFilterPagination<VendorFilters> filter)
    {
        Log.Logger.Information($"Fetching vendor total Pages with filter {filter}");
        var query = Context.Vendors.AsQueryable();
        query = query.Include(x => x.Country);
        query = query.Include(x => x.Contacts);
        query = query.Where(x => x.ClientId == clientId && x.DeletedAt == null);
        return filter.ApplyQuery(query).Count();
    }

    public async Task<int> GetClientVendorsTotalPages(int clientId, RecordFilterPagination<VendorFilters> filter)
    {
        Log.Logger.Information($"Fetching vendor total records with filter {filter}");
        var query = Context.Vendors.AsQueryable();
        query = query.Include(x => x.Country);
        query = query.Include(x => x.Contacts);
        query = query.Where(x => x.ClientId == clientId && x.DeletedAt == null);
        query = filter.ApplyQuery(query);
        return (int)Math.Ceiling(query.Count() / (double)filter.PageSize);
    }

    public async Task<bool> HasExist(int vendorId)
    {
        Log.Logger.Information($"fetch vendor {vendorId}");
        var query = Context.Vendors.Where(x => x.Id == vendorId).FirstOrDefault();
        return query != null;
    }

    public async Task DeleteAllVendorsByClient(int clientId)
    {
        var query = Context.Vendors.Where(x => x.ClientId == clientId);
        foreach (var vendor in query)
        {
            vendor.DeletedAt = SystemClock.Now();
        }
        await Context.Instance.SaveChangesAsync();
    }

    public async Task ClearDeletedVendor()
    {
        var vendorIds = Context.Vendors.Where(x => x.DeletedAt != null && x.DeletedAt <= SystemClock.Now().AddDays(-14));
        var vendorContactsList = Context.VendorContacts.Where(x => vendorIds.Select(w => w.Id).Contains(x.VendorId));
        var matchVendorContactsList = new List<VendorHasContact>();
        foreach (var vendorContact in vendorContactsList)
        {
            var hasVendorContactExistWithinAnyVendors = (from w in Context.VendorContacts
                                                         join v in Context.Vendors on w.VendorId equals v.Id
                                                         where w.ContactId == vendorContact.ContactId && vendorContact.VendorId != w.VendorId && w.Vendor.DeletedAt == null
                                                         select w).Any();
            if (!hasVendorContactExistWithinAnyVendors)
            {
                matchVendorContactsList.Add(vendorContact);
            }
        }
        var contactsList = Context.ProviderContacts.Where(w => matchVendorContactsList.Select(x => x.ContactId).Contains(w.ContactId));
        Context.ProviderContacts.RemoveRange(contactsList);
        Context.VendorContacts.RemoveRange(vendorContactsList);
        Context.Vendors.RemoveRange(vendorIds);
        await Context.Instance.SaveChangesAsync();
    }
}
