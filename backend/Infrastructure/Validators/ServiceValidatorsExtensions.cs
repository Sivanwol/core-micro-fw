using FluentValidation;
using Infrastructure.Validators.Client;
using Infrastructure.Validators.Common;
using Infrastructure.Validators.User;
using Infrastructure.Validators.View;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Validators;

public static class ServiceValidatorsExtensions {
    public static void AddValidatorsExtension(this IServiceCollection services) {

        #region User

        services.AddValidatorsFromAssemblyContaining<ForgetPasswordValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();
        services.AddValidatorsFromAssemblyContaining<ResetPasswordValidator>();
        services.AddValidatorsFromAssemblyContaining<RefreshTokenValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginUserValidator>();
        services.AddValidatorsFromAssemblyContaining<RequestOtpValidator>();
        services.AddValidatorsFromAssemblyContaining<VerifyOTPValidator>();
        services.AddValidatorsFromAssemblyContaining<ConfirmEmailValidator>();
        services.AddValidatorsFromAssemblyContaining<EntityPageValidator>();
        services.AddValidatorsFromAssemblyContaining<EntityFilterItemValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateUserPreferenceValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateUserProfileValidator>();

        #endregion

        #region Client

        services.AddValidatorsFromAssemblyContaining<CreateClientValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateClientContactValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateClientContactValidator>();

        #endregion

        #region View

        services.AddValidatorsFromAssemblyContaining<ViewCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<ViewUpdateColumnsValidator>();
        services.AddValidatorsFromAssemblyContaining<ViewUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<ViewFilterUpdateValidator>();

        #endregion

    }
}