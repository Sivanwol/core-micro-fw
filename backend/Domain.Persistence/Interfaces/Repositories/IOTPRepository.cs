using Domain.Entities;
using Domain.Entities.OTP;
using Infrastructure.Enums;
namespace Domain.Persistence.Interfaces.Repositories;

public interface IOTPRepository : IGenericEmptyRepository<Users> {
    Task<RequestOTPResponseData> RequestOTP(string phoneNumber, int countryId, MFAProvider provider);
    Task<VerifyOTPResponseData> VerifyOTP(string code, string OTPtoken, string UserToken);
    Task<bool> LocateUserByToken(string userToken);
}