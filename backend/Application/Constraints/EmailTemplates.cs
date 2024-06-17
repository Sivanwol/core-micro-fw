namespace Application.Constraints;

public static class EmailTemplates {
    private const string BasePath = "EmailTemplates";
    public const string UserExport = $"{BasePath}/UserExport";
    public const string UserForgotPassword = $"{BasePath}/UserForgotPassword";
    public const string UserRegistration = $"{BasePath}/UserRegistration";
    public const string UserSentOTPCode = $"{BasePath}/UserSentOTPCode";
}