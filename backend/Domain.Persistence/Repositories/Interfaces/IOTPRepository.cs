using Domain.DTO.OTP;
using Domain.Entities;
using Domain.Persistence.Repositories.Common.Interfaces;
using Infrastructure.Enums;
namespace Domain.Persistence.Repositories.Interfaces;

public interface IOTPRepository : IGenericEmptyRepository<ApplicationUser> {
    Task<RequestOTPResponseData> RequestOTPForgetPassword(string homeUrl, string homeImageUrl, Countries country, ApplicationUser user, ApplicationUserOtpCodes codeEntity,
        MFAProvider provider, CancellationToken cancellationToken = default);
    Task<RequestOTPResponseData> RequestOTP(string homeUrl, string homeImageUrl, Countries country, ApplicationUser user, ApplicationUserOtpCodes codeEntity, MFAProvider provider,
        bool isWebLogin = false, CancellationToken cancellationToken = default);
    Task<VerifyOTPResponseData> VerifyOTP(ApplicationUser user, ApplicationUserOtpCodes codeEntity, string code);
}