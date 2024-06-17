using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Domain.Persistence.Context;

public interface IDomainContext : IDisposable {
    DbContext Instance { get; }
    DbSet<Countries> Countries { get; set; }
    DbSet<Languages> Languages { get; set; }
    DbSet<ApplicationUser> Users { get; set; }
    DbSet<ApplicationUserPreferences> UserPreferences { get; set; }
    DbSet<ApplicationUserOtpCodes> UserOtpCodes { get; set; }
    DbSet<Vendors> Vendors { get; set; }
    DbSet<VendorsMetaData> VendorsMetaData { get; set; }
    DbSet<Contacts> Contacts { get; set; }
    DbSet<VendorHasContact> VendorContacts { get; set; }
    DbSet<ProviderHasContact> ProviderContacts { get; set; }
    DbSet<Providers> Providers { get; set; }
    DbSet<ProviderHasProviderCategory> ProviderCategories { get; set; }
    DbSet<ProviderCategory> ProviderCategory { get; set; }
    DbSet<Tags> Tags { get; set; }
    DbSet<ClientHardware> ClientHardware { get; set; }
    DbSet<ClientServers> ClientServers { get; set; }
    DbSet<ClientNetworks> ClientNetworks { get; set; }
    DbSet<ClientServerHasTags> ClientServerTags { get; set; }
    DbSet<ClientServersHasAssets> ClientServersHasAssets { get; set; }
    DbSet<ClientNetworkHasTags> ClientNetworkTags { get; set; }
    DbSet<ClientNetworksHasAssets> ClientNetworksHasAssets { get; set; }
    DbSet<ActivityLog> ActivityLogs { get; set; }
    DbSet<Clients> Clients { get; set; }
    DbSet<Media> Media { get; set; }
    DbSet<Clients> Client { get; set; }
    DbSet<ClientContacts> ClientContacts { get; set; }
    DbSet<ClientEmployees> ClientEmployees { get; set; }
    DbSet<ClientEmployeesHasAssets> ClientEmployeesHasAssets { get; set; }
    DbSet<ClientEmployeeHasTags> ClientEmployeeTags { get; set; }
    DbSet<Assets> Assets { get; set; }
    DbSet<ConfigurableCategories> ConfigurableCategories { get; set; }
    DbSet<ConfigurableEntityColumnDefinition> ConfigurableEntityColumnDefinitions { get; set; }
    DbSet<ConfigurableUserView> ConfigurableUserViews { get; set; }
    DbSet<ConfigurableUserViewHasConfigurableEntityColumnDefinition> ConfigurableUserViewHasConfigurableEntityColumnDefinitions { get; set; }
    DbSet<ConfigurableUserViewTags> ConfigurableUserViewTags { get; set; }
    DbSet<ConfigurableUserViewHasConfigurableUserViewTags> ConfigurableUserViewHasConfigurableUserViewTags { get; set; }
    DbSet<AspNetRoles> AspNetRoles { get; set; }
    DbSet<ConfigurableUserViewsFilters> ConfigurableUserViewsFilters { get; set; }
    DbSet<ConfigurableUserViewsFilterMacros> ConfigurableUserViewsFilterMacros { get; set; }
    Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> withinTransaction, Func<Exception, Task> onException = null);
    Task ExecuteTransactionAsync(Func<Task> withinTransaction, Func<Exception, Task> onException = null);
}