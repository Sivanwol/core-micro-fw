using Domain.Interfaces;
using Domain.Interfaces.Mock;
using Domain.Interfaces.Repositories;
using Domain.Persistence.Mock.Services;
using Domain.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
namespace Domain.Persistence.Extensions; 

public static class ServiceRepositoriesAndMocksExtensions {

    public static void AddRepositoriesExtension(this IServiceCollection services) {
        #region Repositories Registers

        services.AddTransient<ICountriesRepository, CountriesRepository>();
        services.AddTransient<IEthnicitiesRepository, EthnicitiesRepository>();
        services.AddTransient<ILanguagesRepository, LanguagesRepository>();
        services.AddTransient<IReligionsRepository, ReligionsRepository>();
        services.AddTransient<IAppUserRepository, AppUserRepository>();

        #endregion

    }
    public static void AddMocksExtension(this IServiceCollection services) {
        #region Mocks Registers

        services.AddTransient<ICountiesMockService, MockCountiesServices>();
        services.AddTransient<IEthnicitiesMockService, MockEthnicitiesServices>();
        services.AddTransient<ILanguagesMockService, MockLanguagesServices>();
        services.AddTransient<IReligionsMockService, MockReligionsServices>();
        services.AddTransient<IAppUserMockService, MockAppUserServices>();

        #endregion
    }

}