using System.Runtime.Intrinsics.X86;
using MassTransit;
using Domain.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Responses.App.Users;
using MediatR;
using Serilog;
namespace Processor.Services.User; 

public class GetUserProfileHandler  : IRequestHandler<GetUserProfileRequest, ProfileResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;

    public GetUserProfileHandler(IMediator mediator, IAppUserRepository appUserRepository) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
    }

    public async Task<ProfileResponse> Handle(GetUserProfileRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"GetUserProfileHandler: {request.UserId}");
        var result = await _appUserRepository.GetById(request.UserId);
        return await Task.FromResult(new ProfileResponse {
            ID = result.ID,
            FirstName = result.FirstName,
            LastName = result.LastName,
            Email = result.Email,
            PhoneNumber = result.PhoneNumber,
            CountryId = result.CountryId,
            Latitude = result.Latitude,
            Longitude = result.Longitude,
            EmailVerified = result.EmailVerified,
            DefaultImageId = result.DefaultImageId,
            BirthDate = result.BirthDate,
            Gender = result.Gender,
            Height = result.Height,
            MeasureUnits = result.MeasureUnits,
            LanguageId = result.LanguageId,
            ReligionId = result.ReligionId,
            EthnicityId = result.EthnicityId,
            PartnerAgeFrom = result.PartnerAgeFrom,
            PartnerAgeTo = result.PartnerAgeTo,
            PartnerHeightFrom = result.PartnerHeightFrom,
            PartnerHeightTo = result.PartnerHeightTo
        });
    }
}