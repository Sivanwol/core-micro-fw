using System.Security.Claims;
using System.Text;
using Application.Middleware;
using Application.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Application.Extensions;

public static class GenericServiceExtension {
    public static void AddGenericServiceExtension(this IServiceCollection services, IConfiguration configuration,
        Action? moreConfigurationServices) {
        Log.Information("Pre Configure Server");
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddCors(options => {
            options.AddPolicy("AllowAll", builder => {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        services.AddHttpContextAccessor();
        Log.Information("Pre Configure Server Auth");
        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

        Log.Information("Extra Configure Server");
        moreConfigurationServices?.Invoke();

        Log.Information("End Configure Server");
    }

    public static void UseGenericServiceExtension(this IApplicationBuilder app, IWebHostEnvironment env,
        Action? moreConfigurationServices) {
        Log.Information("Pre Bind Configure Server");
        app.UseRouting();
        app.UseMiddleware<ErrorHandlingMiddleware>();

        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }

        app.UseWebSockets();
        app.UseHttpsRedirection();
        Log.Information("Pre Extra Bind Configure Server");
        moreConfigurationServices?.Invoke();
        Log.Information("End Bind Configure Server");
    }
}