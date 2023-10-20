using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Persistence.Mock.Services;
using Domain.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
namespace Domain.Persistence.Extensions;

public static class ServiceRepositoriesAndMocksExtensions {

    public static void AddRepositoriesExtension(this IServiceCollection services) {

        #region Repositories Registers

        services.AddTransient<ICountriesRepository, CountriesRepository>();

        #endregion

    }
    public static void AddMocksExtension(this IServiceCollection services) {
        services.AddTransient<ICountiesMockService, MockCountiesServices>();
    }
}