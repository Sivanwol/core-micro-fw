using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Filters;
using Domain.Persistence.Repositories.Common.Interfaces;
using Infrastructure.Requests.Processor.Services.Provider;

namespace Domain.Persistence.Repositories.Interfaces;

public interface IProviderRepository: IGenericEmptyRepository<Providers>
{
    Task<IEnumerable<Providers>> GetProviders(RecordFilterPagination<ProviderFilters> filter, Guid userId, bool includeEmptyProviderCategories = false, bool onlyShowEmptyProviderCategories = false);
    Task<int> GetProvidersTotalRecords(RecordFilterPagination<ProviderFilters> filter, Guid userId, bool includeEmptyProviderCategories = false, bool onlyShowEmptyProviderCategories = false);
    Task<int> GetProvidersTotalPages(RecordFilterPagination<ProviderFilters> filter, Guid userId, bool includeEmptyProviderCategories = false, bool onlyShowEmptyProviderCategories = false);
    Task<IEnumerable<Providers>> GetClientProviders(RecordFilterPagination<ProviderFilters> filter, int clientId, Guid userId,  bool includeDeleteProviders = false, bool onlyShowDeleteProviders = false);
    Task<int> GetClientProvidersTotalRecords(RecordFilterPagination<ProviderFilters> filter, int clientId, Guid userId, bool includeDeleteProviders = false, bool onlyShowDeleteProviders = false);
    Task<int> GetClientProvidersTotalPages(RecordFilterPagination<ProviderFilters> filter, int clientId, Guid userId,  bool includeDeleteProviders = false, bool onlyShowDeleteProviders = false);
    Task<IEnumerable<ProviderCategory>> GetProviderCategories(RecordFilterPagination<ProviderCategoriesFilters> filter, Guid userId, bool includeEmptyProviderCategories = false, bool onlyShowEmptyProviderCategories = false);
    Task<int> GetProviderCategoriesTotalRecords(RecordFilterPagination<ProviderCategoriesFilters> filter, Guid userId, bool includeEmptyProviderCategories = false, bool onlyShowEmptyProviderCategories = false);
    Task<int> GetProviderCategoriesTotalPages(RecordFilterPagination<ProviderCategoriesFilters> filter, Guid userId, bool includeEmptyProviderCategories = false, bool onlyShowEmptyProviderCategories = false);
    Task RemoveProviderFromCategories(int providerId, List<int> CategoryIds);
    Task AddProviderToCategories(int providerId, List<int> CategoryIds);
    Task<ProviderCategory?> GetProviderCategory(int providerCategoryId);
    Task<bool> HasProviderWithinCategory(int providerCategoryId, int providerId);
    Task<Dictionary<int, int>> GetProviderCategoriesCount(int providerId);
    Task<bool> HasProviderCategoryExist(Guid userId, string name);
    Task<ProviderCategory> CreateProviderCategory(CreateProviderCategory request);
    Task<bool> HasProviderCategoryExistById(int providerCategoryId);
    Task<bool> HasProviderExistWithinCategory(int providerCategoryId, int providerId);
    Task<bool> IsAllowToDeleteProviderCategory(int providerCategoryId);
    Task DeleteProviderCategory(int providerCategoryId);
    Task<Providers?> GetProvider(int providerId);
    Task<Providers> CreateProvider(CreateProviderRequest request, int logoId);
    Task<Providers> CreateProvider(CreateClientProviderRequest request, int logoId);
    Task DeleteProviders(List<int> providerIds);
    Task UpdateProvider(UpdateProviderRequest request, int? logoId);
    Task UpdateProvider(UpdateClientProviderRequest request, int? logoId);
    Task DeleteProvider(int ProviderId);
    Task<bool> HasExist(int ProviderId);
    Task<bool> HasProviderExist(Guid userId, string name, int countyId);
    Task<bool> HasProviderExist(Guid userId, int clientId, string name, int countyId);
    Task<bool> HasUpdateProviderExist(Guid userId, int providerId, string name, int countyId);
    Task<bool> HasUpdateProviderExist(Guid userId, int providerId, int clientId, string name, int countyId);
    Task ClearEmptyProviderCategories();
    Task ClearDeletedProviderCategories();
    Task ClearDeletedProviders();
}
