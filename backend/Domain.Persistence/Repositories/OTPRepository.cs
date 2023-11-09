using Application.Exceptions;
using Domain.Entities.OTP;
using Domain.Persistence.Context;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Interfaces.Repositories;
using Domain.Persistence.Repositories.Common;
using Infrastructure.Enums;
namespace Domain.Persistence.Repositories;

public class OTPRepository : BaseRepository, IOTPRepository {
    private readonly IAppUserRepository _appUserRepository;
    private readonly ICountriesRepository _countriesRepository;
    private readonly IOTPMockService _mock;
    public OTPRepository(
        IDomainContext context,
        ICountriesRepository countriesRepository,
        IAppUserRepository appUserRepository,
        IOTPMockService mock
    ) : base(context) {
        _mock = mock;
        _countriesRepository = countriesRepository;
        _appUserRepository = appUserRepository;
    }

    public async Task<RequestOTPResponseData> RequestOTP(string phoneNumber, int countryId, MFAProvider provider) {
        var countryData = await _countriesRepository.GetById(countryId);
        if (countryData == null) {
            throw new EntityNotFoundException("Countries", countryId.ToString());
        }
        return await Task.FromResult(_mock.RequestOTP(phoneNumber, countryId, provider));
    }
    public Task<VerifyOTPResponseData> VerifyOTP(string code, string OTPtoken, string UserToken) {
        return Task.FromResult(_mock.VerifyOTP(code, OTPtoken, UserToken));
    }
    public Task<bool> LocateUserByToken(string userToken) {
        var user = _appUserRepository.GetUserByToken(userToken);
        if (user == null) {
            throw new EntityNotFoundException("User", userToken);
        }

        return Task.FromResult(true);
    }
}