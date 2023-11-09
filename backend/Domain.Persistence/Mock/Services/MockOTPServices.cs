using Domain.Entities;
using Domain.Entities.OTP;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Mock.Configs;
using Infrastructure.Enums;
namespace Domain.Persistence.Mock.Services;

public class MockOTPServices : IOTPMockService {
    public MockOTPServices() {
        FakerRequester = new RequestOTPMockConfig();
        FakerVerify = new VerifyOTPMockConfig();
    }
    private RequestOTPMockConfig FakerRequester { get; set; }
    private VerifyOTPMockConfig FakerVerify { get; set; }
    private Users User { get; set; }

    public RequestOTPResponseData RequestOTP(string phoneNumber, int countryId, MFAProvider provider) {
        return FakerRequester.Generate(1).First();
    }

    public VerifyOTPResponseData VerifyOTP(string code, string OTPtoken, string UserToken) {
        return FakerVerify.Generate(1).First();
    }
}