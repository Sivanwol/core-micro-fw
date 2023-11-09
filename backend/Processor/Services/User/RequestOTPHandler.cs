using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Responses.Controllers.Auth;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class RequestOtpHandler : IRequestHandler<SendOTPRequest, RequestOTPResponse> {
    private readonly IMediator _mediator;
    private readonly IOTPRepository _otpRepository;

    public RequestOtpHandler(IMediator mediator, IOTPRepository otpRepository) {
        _otpRepository = otpRepository;
        _mediator = mediator;
    }

    public async Task<RequestOTPResponse> Handle(SendOTPRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"RequestOtpHandler: [{request.Provider},{request.CountryId}, {request.PhoneNumber}]");
        var result = await _otpRepository.RequestOTP(request.PhoneNumber, request.CountryId, request.Provider);
        return await Task.FromResult(new RequestOTPResponse {
            UserToken = result.UserToken,
            OTPExpired = result.OTPExpired,
            OTPToken = result.OTPToken,
            IsLocked = false
        });
    }
}