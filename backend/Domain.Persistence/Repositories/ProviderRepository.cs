

using System.Linq.Dynamic.Core;
using Application.Constraints;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Filters;
using Domain.Persistence.Context;
using Domain.Persistence.Repositories.Common;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Requests.Processor.Services.Provider;
using Infrastructure.Services.S3;
using Microsoft.EntityFrameworkCore;
using Serilog;

internal class ProviderRepository : BaseRepository, IProviderRepository
{
    private readonly IStorageControlService _storageControlService;
    private readonly IClientRepository _clientRepository;
    private readonly IMediaRepository _mediaRepository;
    public ProviderRepository(IDomainContext context,
    IStorageControlService storageControlService,
    IClientRepository clientRepository,
    IMediaRepository mediaRepository) : base(context)
    {
        _storageControlService = storageControlService;
        _mediaRepository = mediaRepository;
        _clientRepository = clientRepository;
    }

    public async Task AddProviderToCategories(int providerId, List<int> categoryIds)
    {
        foreach (var categoryId in categoryIds)
        {
            if (!await HasProviderWithinCategory(categoryId, providerId))
            {
                Context.ProviderCategories.Add(new ProviderHasProviderCategory { CategoryId = categoryId, ProviderId = providerId });
            }
        }
        await Context.Instance.SaveChangesAsync();
    }

    public async Task<bool> HasProviderWithinCategory(int providerCategoryId, int providerId)
    {
        var category = from x in Context.ProviderCategories
                       join c in Context.ProviderCategory on x.CategoryId equals c.Id
                       join p in Context.Providers on x.ProviderId equals p.Id
                       where c.Id == providerCategoryId && p.Id == providerId && c.DeletedAt == null && p.DeletedAt == null
                       select x;
        return await category.AnyAsync();
    }

    public async Task ClearDeletedProviders()
    {
        var providers = from x in Context.Providers
        where x.DeletedAt != null && x.DeletedAt < SystemClock.Now().AddDays(-14)
        select x;
        foreach (var provider in providers) {
            if (provider.LogoId.HasValue)
            {
                // var media = await _mediaRepository.GetMedia(provider.LogoId.Value);
                // we deleted the media record
                await _mediaRepository.RemoveMedia(new List<int> { provider.LogoId.Value });
                // we clear the storage for any media files
                await _storageControlService.DeleteFolder(StorageAws.BucketGlobalMedia, StorageGlobalPath.Provider(provider.Id));
            }
            // TODO: need add even flow here for provider deletion
        }
        Context.Providers.RemoveRange(providers);
        await Context.Instance.SaveChangesAsync();
    }

    public async Task ClearDeletedProviderCategories()
    {
        var categories = from x in Context.ProviderCategory
                         where x.DeletedAt != null && x.DeletedAt < SystemClock.Now().AddDays(-14) &&
                         (from y in Context.ProviderCategories where y.CategoryId == x.Id select y.ProviderId).Count() == 0
                         select x;
        Context.ProviderCategory.RemoveRange(categories);
        await Context.Instance.SaveChangesAsync();
    }
    public async Task ClearEmptyProviderCategories()
    {
        var categories = from x in Context.ProviderCategory
                         where x.DeletedAt == null &&
                         (from y in Context.ProviderCategories where y.CategoryId == x.Id select y.ProviderId).Count() == 0
                         select x;
        foreach (var category in categories)
        {
            category.DeletedAt = SystemClock.Now();
        }
        await Context.Instance.SaveChangesAsync();
    }

    public async Task<Providers> CreateProvider(CreateProviderRequest request, int logoId)
    {
        if (!await HasProviderExist(request.UserId, request.Name, request.CountryId))
        {
            throw new EntityFoundException("Provider", $"[{request.UserId}, {request.Name} , {request.CountryId}]");
        }
        if (!await _mediaRepository.MediaExists(logoId))
        {
            throw new EntityNotFoundException("Media", $"[{logoId}]");
        }
        var provider = new Providers
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
            ProviderType = request.ProviderType
        };
        Context.Providers.Add(provider);
        await Context.Instance.SaveChangesAsync();
        return provider;
    }

    public async Task<Providers> CreateProvider(CreateClientProviderRequest request, int logoId)
    {
        if (!await HasProviderExist(request.UserId, request.Name, request.CountryId))
        {
            throw new EntityFoundException("Provider", $"[{request.UserId}, {request.Name} , {request.CountryId}]");
        }
        if (!await _mediaRepository.MediaExists(logoId))
        {
            throw new EntityNotFoundException("Media", $"[{logoId}]");
        }
        if (!await _clientRepository.IsClientExists(request.ClientId))
        {
            throw new EntityNotFoundException("Client", request.ClientId.ToString());
        }
        var provider = new Providers
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
            ProviderType = request.ProviderType
        };
        Context.Providers.Add(provider);
        await Context.Instance.SaveChangesAsync();
        return provider;
    }

    public async Task<ProviderCategory> CreateProviderCategory(CreateProviderCategory request)
    {
        if (await HasProviderCategoryExist(request.UserId, request.Name))
        {
            throw new EntityFoundException("ProviderCategory", $"[request.UserId, '{request.Name}'] already exist");
        }
        var providerCategory = new ProviderCategory
        {
            UserId = request.UserId.ToString(),
            Name = request.Name,
            Description = request.Description
        };
        Context.ProviderCategory.Add(providerCategory);
        await Context.Instance.SaveChangesAsync();
        return providerCategory;
    }

    public async Task<bool> HasProviderCategoryExist(Guid userId, string name)
    {
        var located = await Context.ProviderCategory.FirstOrDefaultAsync(x => x.UserId == userId.ToString() && x.Name == name);
        return located != null;
    }
    public async Task DeleteProvider(int providerId)
    {
        var provider = await GetProvider(providerId);
        if (provider == null)
        {
            throw new EntityNotFoundException("Provider", providerId);
        }
        provider.DeletedAt = SystemClock.Now();
        await Context.Instance.SaveChangesAsync();
    }

    public async Task DeleteProviderCategory(int providerCategoryId)
    {
        var providerCategory = await GetProviderCategory(providerCategoryId);
        if (providerCategory == null)
        {
            throw new EntityNotFoundException("ProviderCategory", providerCategoryId);
        }
        providerCategory.DeletedAt = SystemClock.Now();
        await Context.Instance.SaveChangesAsync();
    }

    public async Task DeleteProviders(List<int> providerIds)
    {
        foreach (var providerId in providerIds)
        {
            var provider = await GetProvider(providerId);
            if (provider != null)
            {
                provider.DeletedAt = SystemClock.Now();
            }
        }
        await Context.Instance.SaveChangesAsync();
    }

    public async Task<Providers?> GetProvider(int ProviderId)
    {
        return await Context.Providers.Where(x => x.Id == ProviderId && x.DeletedAt == null).FirstOrDefaultAsync();
    }

    public Task<Dictionary<int, int>> GetProviderCategoriesCount(int providerId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProviderCategory>> GetProviderCategories(RecordFilterPagination<ProviderCategoriesFilters> filter, Guid userId, bool includeEmptyProviderCategories = false, bool onlyShowEmptyProviderCategories = false)
    {
        Log.Logger.Information($"Fetching Provider Categories records with filter {filter}");
        var query = Context.ProviderCategory.AsQueryable();
        query = filter.ApplyQuery(query);

        query = query.Where(x => x.UserId == userId.ToString());

        if (onlyShowEmptyProviderCategories)
        {
            query = query.Where(x => x.DeletedAt != null);
        }
        else
        {
            if (!includeEmptyProviderCategories)
            {
                query = query.Where(x => x.DeletedAt == null);
            }
        }

        return query.ToList();
    }

    public async Task<int> GetProviderCategoriesTotalPages(RecordFilterPagination<ProviderCategoriesFilters> filter, Guid userId, bool includeEmptyProviderCategories = false, bool onlyShowEmptyProviderCategories = false)
    {
        Log.Logger.Information($"Fetching Provider Categories total Pages with filter {filter}");
        var query = Context.ProviderCategory.AsQueryable();
        query = filter.ApplyQuery(query);

        query = query.Where(x => x.UserId == userId.ToString());

        if (onlyShowEmptyProviderCategories)
        {
            query = query.Where(x => x.DeletedAt != null);
        }
        else
        {
            if (!includeEmptyProviderCategories)
            {
                query = query.Where(x => x.DeletedAt == null);
            }
        }
        return (int)Math.Ceiling(query.Count() / (double)filter.PageSize);
    }

    public async Task<int> GetProviderCategoriesTotalRecords(RecordFilterPagination<ProviderCategoriesFilters> filter, Guid userId, bool includeEmptyProviderCategories = false, bool onlyShowEmptyProviderCategories = false)
    {
        Log.Logger.Information($"Fetching Provider Categories total records with filter {filter}");
        var query = Context.ProviderCategory.AsQueryable();
        query = filter.ApplyQuery(query);

        query = query.Where(x => x.UserId == userId.ToString());

        if (onlyShowEmptyProviderCategories)
        {
            query = query.Where(x => x.DeletedAt != null);
        }
        else
        {
            if (!includeEmptyProviderCategories)
            {
                query = query.Where(x => x.DeletedAt == null);
            }
        }
        return query.Count();
    }

    public async Task<ProviderCategory?> GetProviderCategory(int providerCategoryId)
    {
        var providerCategory = Context.ProviderCategory.FirstOrDefault(x => x.Id == providerCategoryId && x.DeletedAt == null);
        return providerCategory;
    }

    public async Task<IEnumerable<Providers>> GetProviders(RecordFilterPagination<ProviderFilters> filter, Guid userId, bool includeDeleteProviders = false, bool onlyShowDeleteProviders = false)
    { 
        Log.Logger.Information($"Fetching Providers records with filter {filter}");
        var query = Context.Providers.AsQueryable();
        query = filter.ApplyQuery(query);

        query = query.Where(x => x.UserId == userId.ToString());

        if (onlyShowDeleteProviders)
        {
            query = query.Where(x => x.DeletedAt != null);
        }
        else
        {
            if (!includeDeleteProviders)
            {
                query = query.Where(x => x.DeletedAt == null);
            }
        }
        return query.ToList();
    }

    public async Task<int> GetProvidersTotalPages(RecordFilterPagination<ProviderFilters> filter, Guid userId, bool includeDeleteProviders = false, bool onlyShowDeleteProviders = false)
    {
        Log.Logger.Information($"Fetching Providers total pages with filter {filter }by user id {userId}");
        var query = Context.Providers.AsQueryable();
        query = filter.ApplyQuery(query);

        query = query.Where(x => x.UserId == userId.ToString());

        if (onlyShowDeleteProviders)
        {
            query = query.Where(x => x.DeletedAt != null);
        }
        else
        {
            if (!includeDeleteProviders)
            {
                query = query.Where(x => x.DeletedAt == null);
            }
        }
        return (int)Math.Ceiling(query.Count() / (double)filter.PageSize);
    }

    public async Task<int> GetProvidersTotalRecords(RecordFilterPagination<ProviderFilters> filter, Guid userId, bool includeDeleteProviders = false, bool onlyShowDeleteProviders = false)
    {
        Log.Logger.Information($"Fetching Providers total records with filter {filter} by user id {userId}");
        var query = Context.Providers.AsQueryable();
        query = filter.ApplyQuery(query);

        query = query.Where(x => x.UserId == userId.ToString());

        if (onlyShowDeleteProviders)
        {
            query = query.Where(x => x.DeletedAt != null);
        }
        else
        {
            if (!includeDeleteProviders)
            {
                query = query.Where(x => x.DeletedAt == null);
            }
        }
        return query.Count();
    }
    public async Task<IEnumerable<Providers>> GetClientProviders(RecordFilterPagination<ProviderFilters> filter, int clientId, Guid userId,  bool includeDeleteProviders = false, bool onlyShowDeleteProviders = false)
    {
        Log.Logger.Information($"Fetching Providers records with filter {filter} by client id {clientId} and user id {userId}");
        var query = Context.Providers.AsQueryable();
        query = filter.ApplyQuery(query);

        query = query.Where(x => x.UserId == userId.ToString() && x.ClientId == clientId);

        if (onlyShowDeleteProviders)
        {
            query = query.Where(x => x.DeletedAt != null);
        }
        else
        {
            if (!includeDeleteProviders)
            {
                query = query.Where(x => x.DeletedAt == null);
            }
        }
        return query.ToList();
    }

    public async Task<int> GetClientProvidersTotalPages(RecordFilterPagination<ProviderFilters> filter, int clientId, Guid userId,  bool includeDeleteProviders = false, bool onlyShowDeleteProviders = false)
    {
        Log.Logger.Information($"Fetching Providers total pages with filter {filter} by client id {clientId} and user id {userId}");
        var query = Context.Providers.AsQueryable();
        query = filter.ApplyQuery(query);

        query = query.Where(x => x.UserId == userId.ToString() && x.ClientId == clientId);

        if (onlyShowDeleteProviders)
        {
            query = query.Where(x => x.DeletedAt != null);
        }
        else
        {
            if (!includeDeleteProviders)
            {
                query = query.Where(x => x.DeletedAt == null);
            }
        }
        return (int)Math.Ceiling(query.Count() / (double)filter.PageSize);
    }

    public async Task<int> GetClientProvidersTotalRecords(RecordFilterPagination<ProviderFilters> filter, int clientId, Guid userId,  bool includeDeleteProviders = false, bool onlyShowDeleteProviders = false)
    {
        Log.Logger.Information($"Fetching Providers total records with filter {filter} by client id {clientId} and user id {userId}");
        var query = Context.Providers.AsQueryable();
        query = filter.ApplyQuery(query);

        query = query.Where(x => x.UserId == userId.ToString() && x.ClientId == clientId);

        if (onlyShowDeleteProviders)
        {
            query = query.Where(x => x.DeletedAt != null);
        }
        else
        {
            if (!includeDeleteProviders)
            {
                query = query.Where(x => x.DeletedAt == null);
            }
        }
        return query.Count();
    }

    public async Task<bool> HasExist(int ProviderId)
    {
        return await Context.Providers.Where(x => x.Id == ProviderId && x.DeletedAt == null).AnyAsync();
    }

    public async Task<bool> HasProviderCategoryExistById(int providerCategoryId)
    {
        return await Context.ProviderCategory.AnyAsync(x => x.Id == providerCategoryId && x.DeletedAt == null);
    }

    public async Task<bool> HasProviderExist(Guid userId, string name, int countyId)
    {
        return await Context.Providers.AnyAsync(x => x.UserId == userId.ToString() && x.Name == name && x.CountryId == countyId && x.DeletedAt == null);
    }

    public async Task<bool> HasProviderExist(Guid userId, int clientId, string name, int countyId)
    {
        return await Context.Providers.AnyAsync(x => x.UserId == userId.ToString() && x.ClientId == clientId && x.CountryId == countyId && x.DeletedAt == null);
    }

    public async Task<bool> HasProviderExistWithinCategory(int providerCategoryId, int providerId)
    {
        return await Context.ProviderCategories
        .Include(x => x.Category)
        .Include(x => x.Provider)
        .AnyAsync(x => x.ProviderId == providerId && x.CategoryId == providerCategoryId && x.Category.DeletedAt == null && x.Provider.DeletedAt == null);
    }

    public async Task<bool> HasUpdateProviderExist(Guid userId, int providerId, string name, int countyId)
    {
        return await Context.Providers.AnyAsync(x => x.UserId == userId.ToString() && x.Id != providerId && x.Name == name && x.CountryId == countyId && x.DeletedAt == null);
    }

    public async Task<bool> HasUpdateProviderExist(Guid userId, int providerId, int clientId, string name, int countyId)
    {
        return await Context.Providers.AnyAsync(x => x.UserId == userId.ToString() && x.Id != providerId && x.ClientId == clientId && x.Name == name && x.CountryId == countyId && x.DeletedAt == null);
    }

    public async Task<bool> IsAllowToDeleteProviderCategory(int providerCategoryId)
    {
        var allowed = from x in Context.ProviderCategory
                      where x.DeletedAt == null &&
                      x.Id == providerCategoryId &&
                      (from y in Context.ProviderCategories where y.CategoryId == x.Id select y.ProviderId).Count() != 0
                      select x;
        return await allowed.AnyAsync();
    }

    public async Task RemoveProviderFromCategories(int providerId, List<int> categoryIds)
    {

        var categoriesToRemove = new List<Int32>();
        foreach (var categoryId in categoryIds)
        {
            if (await HasProviderWithinCategory(categoryId, providerId))
            {
                categoriesToRemove.Add(categoryId);
            }
        }
        var categories = from x in Context.ProviderCategories
                         join c in Context.ProviderCategory on x.CategoryId equals c.Id
                         join p in Context.Providers on x.ProviderId equals p.Id
                         where categoriesToRemove.Contains(x.CategoryId) && p.Id == providerId && c.DeletedAt == null && p.DeletedAt == null
                         select x;
        Context.ProviderCategories.RemoveRange(categories);

        await Context.Instance.SaveChangesAsync();
    }

    public async Task UpdateProvider(UpdateProviderRequest request, int? logoId)
    {
        if (!await HasProviderExist(request.UserId, request.Name, request.CountryId))
        {
            throw new EntityFoundException("Provider", $"[{request.UserId}, {request.Name} , {request.CountryId}]");
        }
        if (logoId.HasValue)
        {
            if (!await _mediaRepository.MediaExists(logoId.Value))
            {
                throw new EntityNotFoundException("Media", $"[{logoId}]");
            }
        }
        var provider = await GetProvider(request.ProviderId);
        if (provider == null)
        {
            throw new EntityNotFoundException("Provider", request.ProviderId.ToString());
        }

        provider.Name = request.Name;
        provider.Description = request.Description;
        provider.Address = request.Address;
        provider.CountryId = request.CountryId;
        provider.SiteUrl = request.SiteUrl;
        if (logoId != null)
        {
            provider.LogoId = logoId.Value;
        }
        provider.SupportEmail = request.SupportEmail;
        provider.SupportUrl = request.SupportUrl;
        provider.SupportPhone = request.SupportPhone;
        provider.ProviderType = request.ProviderType;
        await Context.Instance.SaveChangesAsync();
    }

    public async Task UpdateProvider(UpdateClientProviderRequest request, int? logoId)
    {
        if (!await HasProviderExist(request.UserId, request.Name, request.CountryId))
        {
            throw new EntityFoundException("Provider", $"[{request.UserId}, {request.Name} , {request.CountryId}]");
        }
        if (logoId.HasValue)
        {
            if (!await _mediaRepository.MediaExists(logoId.Value))
            {
                throw new EntityNotFoundException("Media", $"[{logoId}]");
            }
        }
        var provider = await GetProvider(request.ProviderId);
        if (provider == null)
        {
            throw new EntityNotFoundException("Provider", request.ProviderId.ToString());
        }
        provider.ClientId = request.ClientId;
        provider.Name = request.Name;
        provider.Description = request.Description;
        provider.Address = request.Address;
        provider.CountryId = request.CountryId;
        provider.SiteUrl = request.SiteUrl;
        if (logoId != null)
        {
            provider.LogoId = logoId.Value;
        }
        provider.SupportEmail = request.SupportEmail;
        provider.SupportUrl = request.SupportUrl;
        provider.SupportPhone = request.SupportPhone;
        provider.ProviderType = request.ProviderType;
        await Context.Instance.SaveChangesAsync();
    }
}