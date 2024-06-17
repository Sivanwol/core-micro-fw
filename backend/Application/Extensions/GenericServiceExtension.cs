using Application.Middleware;
using Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
namespace Application.Extensions;

public static class GenericServiceExtension {
    public static void AddGenericServiceExtension(this IServiceCollection services, IConfiguration configuration,
        Action? moreConfigurationServices) {
        Log.Information("Pre Configure Server");
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddCors(options => {
            options.AddPolicy("AllowAll", builder => {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        services.AddControllers().AddNewtonsoftJson(options => {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            options.SerializerSettings.Formatting = Formatting.Indented;
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
        });
        ;
        ;
        services.AddEndpointsApiExplorer();
        services.Configure<ForwardedHeadersOptions>(options => {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
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
        app.UseSerilogRequestLogging();
        AppUrlHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
        app.UseMiddleware<ErrorHandlingMiddleware>();
        // app.UseMiddleware<CorrelationMiddleware>();

        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        app.UseForwardedHeaders(new ForwardedHeadersOptions {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        app.UseCors("AllowAll");
        app.UseWebSockets();
        app.UseHttpsRedirection();
        Log.Information("Pre Extra Bind Configure Server");
        moreConfigurationServices?.Invoke();
        Log.Information("End Bind Configure Server");
    }
}