using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Filters;
using Domain.Persistence.Repositories.Common.Interfaces;
using Infrastructure.Requests.Processor.Services.Vendor;

namespace Domain.Persistence.Repositories.Interfaces;

public interface IVendorRepository: IGenericEmptyRepository<Vendors>
{
    Task<IEnumerable<Vendors>> GetVendors(RecordFilterPagination<VendorFilters> filter);
    Task<int> GetVendorsTotalRecords(RecordFilterPagination<VendorFilters> filter);
    Task<int> GetVendorsTotalPages(RecordFilterPagination<VendorFilters> filter);
    Task<Vendors?> GetVendor(int vendorId);
    Task<Vendors> CreateVendor(CreateVendorRequest request, int logoId);
    Task<Vendors> CreateVendor(CreateClientVendorRequest request, int logoId);
    Task DeleteVendors(List<int> vendorIds);
    Task UpdateVendor(UpdateVendorRequest request, int? logoId);
    Task UpdateVendor(UpdateClientVendorRequest request, int? logoId);
    Task<bool> HasExist(int vendorId);
    Task<bool> HasVendorExist(Guid userId, string name, int countyId);
    Task<bool> HasVendorExist(Guid userId, int clientId, string name, int countyId);
    Task<bool> HasUpdateVendorExist(Guid userId, int vendorId, string name, int countyId);
    Task<bool> HasUpdateVendorExist(Guid userId, int vendorId, int clientId, string name, int countyId);
    Task<IEnumerable<Vendors>> GetClientVendors(int clientId, RecordFilterPagination<VendorFilters> filter);
    Task<int> GetClientVendorsTotalRecords(int clientId, RecordFilterPagination<VendorFilters> filter);
    Task<int> GetClientVendorsTotalPages(int clientId, RecordFilterPagination<VendorFilters> filter);
    Task ClearDeletedVendor();
    Task DeleteAllVendorsByClient(int clientId);
}
