using FluentValidation;
using Infrastructure.Validators.Account;
using Infrastructure.Validators.Backoffice.Account;
using Infrastructure.Validators.Chat;
using Infrastructure.Validators.Session;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Validators;

public static class ServiceValidatorsExtensions {
    public static void AddValidatorsExtension(this IServiceCollection services) {

        #region Validations

        services.AddValidatorsFromAssemblyContaining<ForgetPasswordValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();
        services.AddValidatorsFromAssemblyContaining<ResetPasswordValidator>();
        services.AddValidatorsFromAssemblyContaining<RefreshTokenValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginUserValidator>();
        services.AddValidatorsFromAssemblyContaining<SendCodeToProviderValidator>();
        services.AddValidatorsFromAssemblyContaining<SendCodeFromProviderValidator>();
        // account auth
        services.AddValidatorsFromAssemblyContaining<RequestOtpValidator>();
        services.AddValidatorsFromAssemblyContaining<VerifyOTPValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterNewUserInitValidator>();
        // account register
        services.AddValidatorsFromAssemblyContaining<RegisterNewUserInfoValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterNewUserPreferenceValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterNewUserFilesValidator>();
        // account update
        services.AddValidatorsFromAssemblyContaining<UpdateUserProfileValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateUserFilesValidator>();
        // session operations
        services.AddValidatorsFromAssemblyContaining<StartSessionValidator>();
        services.AddValidatorsFromAssemblyContaining<SessionQuestionAnswerValidator>();
        services.AddValidatorsFromAssemblyContaining<SessionQuestionRateValidator>();
        services.AddValidatorsFromAssemblyContaining<SessionMarkUnmatchValidator>();
        services.AddValidatorsFromAssemblyContaining<SessionReplyToQuestionValidator>();
        // chat operations
        services.AddValidatorsFromAssemblyContaining<ChatSendMessageValidator>();
        services.AddValidatorsFromAssemblyContaining<ChatEditMessageValidator>();
        services.AddValidatorsFromAssemblyContaining<ChatDeleteMessageValidator>();
        services.AddValidatorsFromAssemblyContaining<ChatFlagUserValidator>();

        #endregion

    }
}