namespace Domain.DTO.OTP;

public class RequestOTPResponseData {
    public string OTPToken { get; set; }
    public string UserToken { get; set; }
    public DateTime OTPExpired { get; set; }
}