using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Responses.Controllers.User;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class GetUserProfileHandler : IRequestHandler<GetUserProfileRequest, ProfileResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediaRepository _mediaRepository;
    private readonly IMediator _mediator;

    public GetUserProfileHandler(IMediator mediator,
        IAppUserRepository appUserRepository,
        IMediaRepository mediaRepository) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
        _mediaRepository = mediaRepository;
    }

    public async Task<ProfileResponse> Handle(GetUserProfileRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"GetUserProfileHandler: {request.UserId}");
        var result = await _appUserRepository.GetUserProfile(request.UserId);
        var resultFiles = await _mediaRepository.GetUserMedia(request.UserId);
        return await Task.FromResult(new ProfileResponse {
            ID = result.User.UserId,
            FirstName = result.User.FirstName,
            LastName = result.User.LastName,
            Email = result.User.Email,
            PhoneNumber = result.User.PhoneNumber,
            CountryId = result.User.CountryId,
            Latitude = result.User.Latitude,
            Longitude = result.User.Longitude,
            EmailVerified = result.User.EmailVerified,
            DefaultImageId = result.User.DefaultImageId,
            TermsApproved = result.User.TermsApproved,
            BirthDate = result.User.BirthDate,
            Gender = Enum.GetName(result.User.Gender)!,
            Height = result.User.Height,
            MeasureUnits = Enum.GetName(result.User.MeasureUnits)!,
            LanguageId = result.User.LanguageId,
            ReligionId = result.User.ReligionId,
            EthnicityId = result.User.EthnicityId,
            PartnerAgeFrom = result.User.PartnerAgeFrom,
            PartnerAgeTo = result.User.PartnerAgeTo,
            PartnerHeightFrom = result.User.PartnerHeightFrom,
            PartnerHeightTo = result.User.PartnerHeightTo,
            PartnerReligions = result.PartnerReligions.Select(x => x.ReligionId),
            PartnerEthnicities = result.PartnerEthnicities.Select(x => x.EthnicityId),
            Files = resultFiles.Select(x => x.ToMediaInfo())
        });
    }
}