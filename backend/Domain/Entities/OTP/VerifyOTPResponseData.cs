namespace Domain.Entities.OTP;

public class VerifyOTPResponseData {
    public Users? User { get; set; }
    public bool IsValid { get; set; }
}