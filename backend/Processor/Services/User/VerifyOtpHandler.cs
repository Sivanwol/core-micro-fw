using System.Security.Claims;
using Application.Utils;
using Domain.Persistence.Interfaces.Repositories;
using Domain.Requests.Processor.Services.User;
using Infrastructure.Responses.Controllers.Auth;
using Infrastructure.Services.Auth;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class VerifyOtpHandler : IRequestHandler<VerifyOtpUserRequest, VerifyOTPResponse> {
    private readonly IJwtAuthManager _jwtAuthManager;
    private readonly IMediator _mediator;
    private readonly IOTPRepository _otpRepository;

    public VerifyOtpHandler(IMediator mediator,
        IJwtAuthManager jwtAuthManager,
        IOTPRepository otpRepository) {
        _otpRepository = otpRepository;
        _jwtAuthManager = jwtAuthManager;
        _mediator = mediator;
    }

    public async Task<VerifyOTPResponse> Handle(VerifyOtpUserRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"VerifyOtpHandler: [{request.OTPToken}, {request.UserToken}]");
        var result = await _otpRepository.VerifyOTP(request.Code, request.OTPToken, request.UserToken);
        return new VerifyOTPResponse {
            UserId = result.IsValid ? result.User?.UserId : null,
            Tokens = result.IsValid ? _jwtAuthManager.GenerateTokens(request.UserToken, new List<Claim>(), SystemClock.Now().DateTime) : null
        };
    }
}