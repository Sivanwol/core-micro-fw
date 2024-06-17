namespace Application.Utils;

public static class ActivityLogOperationType
{

    #region User

    public const string UserLogin = "UserLogin";
    public const string UserLogout = "UserLogout";
    public const string UserRegister = "UserRegister";
    public const string UserUpdate = "UserUpdate";
    public const string UserDelete = "UserDelete";
    public const string UserExport = "UserExport";
    public const string UserChangePassword = "UserChangePassword";
    public const string UserResetPassword = "UserResetPassword";
    public const string UserForgotPassword = "UserForgotPassword";
    public const string UserSendOTP = "UserSendOTP";
    public const string UserVerifyOTP = "UserVerifyOTP";
    public const string UserUpdatePreference = "UserUpdatePreference";
    public const string UserResetPreference = "UserResetPreference";

    #endregion

    #region User Operations

    public const string UserCreate = "UserCreate";
    public const string UserForceUpdate = "UserForceUpdate";
    public const string ListOfUsers = "ListOfUsers";

    #endregion

    #region Client Operations

    public const string ClientCreate = "ClientCreate";
    public const string ClientUpdate = "ClientUpdate";
    public const string ClientDelete = "ClientDelete";
    public const string ClientExport = "ClientExport";
    public const string ClientList = "ClientList";
    public const string ClientContactCreate = "ClientContactCreate";
    public const string ClientContactUpdate = "ClientContactUpdate";
    public const string ClientContactDelete = "ClientContactDelete";
    public const string ClientContactList = "ClientContactList";

    #endregion

    #region Vendor Operations

    public const string VendorCreate = "VendorCreate";
    public const string VendorUpdate = "VendorUpdate";
    public const string VendorDelete = "VendorDelete";

    #endregion
}