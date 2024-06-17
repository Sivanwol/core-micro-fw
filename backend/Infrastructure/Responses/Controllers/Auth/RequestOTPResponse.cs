namespace Infrastructure.Responses.Controllers.Auth;

public class RequestOTPResponse {

    /// <summary>
    ///     registered OTP Token
    /// </summary>
    public string OTPToken { get; set; }

    /// <summary>
    ///     User Token
    /// </summary>
    public string UserToken { get; set; }

    /// <summary>
    ///     when the OTP will be expired
    /// </summary>
    public DateTime OTPExpired { get; set; }

    /// <summary>
    ///     the user is locked or not
    /// </summary>
    public bool HasSent { get; set; }
}