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
        string domain, Action? moreConfigurationServices) {

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
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
            options.Authority = domain;
            options.Audience = configuration["Auth0:Audience"];
            options.TokenValidationParameters = new TokenValidationParameters {
                NameClaimType = ClaimTypes.NameIdentifier,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth0:SigningKey"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true
            };
        });
        services.AddAuthorization(options => {
            Scopes.GetScopes().ForEach(scope => {
                options.AddPolicy(scope, policy => policy.Requirements.Add(
                    new HasScopeRequirement(scope, domain)));
            });
        });
        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

        Log.Information("Extra Configure Server");
        moreConfigurationServices?.Invoke();

        Log.Information("End Configure Server");
    }

    public static void  UseGenericServiceExtension(this IApplicationBuilder app, IWebHostEnvironment env, Action? moreConfigurationServices) {
        
        Log.Information("Pre Bind Configure Server");
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
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