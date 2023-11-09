using Domain.Entities.OTP;
using Infrastructure.Enums;
namespace Domain.Persistence.Interfaces.Mock {
    public interface IOTPMockService {
        RequestOTPResponseData RequestOTP(string phoneNumber, int countryId, MFAProvider provider);
        VerifyOTPResponseData VerifyOTP(string code, string OTPtoken, string UserToken);
    }
}