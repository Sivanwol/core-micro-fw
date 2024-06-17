using Domain.Persistence.Mock.Services;
using Domain.Persistence.Mock.Services.Interfaces;
using Domain.Persistence.Repositories;
using Domain.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace Domain.Persistence;

public static class ServiceRepositoriesAndMocksExtensions {

    public static void AddRepositoriesExtension(this IServiceCollection services) {

        #region Repositories Registers

        services.AddTransient<ICountriesRepository, CountriesRepository>();
        services.AddTransient<ILanguagesRepository, LanguagesRepository>();
        services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
        services.AddTransient<IOTPRepository, OTPRepository>();
        services.AddTransient<IActivityLogRepository, ActivityLogRepository>();
        services.AddTransient<IConfigurableUserViewRepository, ConfigurableUserViewRepository>();
        services.AddTransient<IClientRepository, ClientRepository>();
        services.AddTransient<IMediaRepository, MediaRepository>();
        services.AddTransient<IVendorRepository, VendorRepository>();

        #endregion

    }
    public static void AddMocksExtension(this IServiceCollection services) {

        #region Mocks Registers

        services.AddTransient<ICountiesMockService, MockCountiesServices>();
        services.AddTransient<ILanguagesMockService, MockLanguagesServices>();
        services.AddTransient<IAppUserMockService, MockAppUserServices>();
        services.AddTransient<IOTPMockService, MockUserOPTCodeServices>();
        services.AddTransient<IActivityLogMockService, MockActivityLogService>();

        #endregion

    }
}