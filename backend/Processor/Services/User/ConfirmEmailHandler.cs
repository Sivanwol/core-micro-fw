using Application.Constraints;
using Application.Exceptions;
using Application.Responses.Base;
using Application.Utils;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Domain.Utils;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;
namespace Processor.Services.User;

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailRequest, EmptyResponse> {
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmEmailHandler(IMediator mediator,
        IActivityLogRepository activityLogRepository,
        UserManager<ApplicationUser> userManager) {
        _mediator = mediator;
        _activityLogRepository = activityLogRepository;
        _userManager = userManager;
    }

    public async Task<EmptyResponse> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"LogoutUserHandler: [{request.UserToken}]");
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) {
            throw new EntityNotFoundException("User", request.UserId.ToString());
        }
        if (user.Token != request.UserToken) {
            throw new AuthenticationException("Invalid user not matching the security token");
        }
        if (request.LoggedUserId == Guid.Empty || request.UserId == Guid.Empty) {
            throw new InvalidRequestException("Invalid user id and/or logged user id provided");
        }
        if (request.LoggedUserId == request.UserId && string.IsNullOrEmpty(request.Code)) {
            throw new InvalidRequestException("Code is required");
        }
        var code = request.Code;
        if (request.UserId != request.LoggedUserId) {
            var loggedUser = await _userManager.FindByIdAsync(request.LoggedUserId.ToString());
            if (loggedUser == null) {
                throw new EntityNotFoundException("User", request.LoggedUserId.ToString());
            }

            var hasMatchingRoles = await AuthUtils.HasMathchingRoles(_userManager, loggedUser, [Roles.Admin]);
            if (!hasMatchingRoles) {
                throw new AuthenticationException("Invalid user not matching permissions");
            }

            code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded) {
            result.Errors.ToList().ForEach(e => Log.Logger.Error("unable confirm email - error: {0}", e.Description ?? ""));
            throw new AuthenticationException("unable confirm email");
        }
        user.RegisterCompletedAt = SystemClock.Now();
        await _userManager.UpdateAsync(user);
        Log.Logger.Information("New Register User email confirmed -  {UserId}", user.Id);
        await _activityLogRepository.AddActivity(
            Guid.Parse(user.Id),
            user.Id,
            nameof(ApplicationUser),
            ActivityLogOperationType.UserRegister,
            "User Request Registered", "User Request Registered",
            ActivityStatus.Success);
        return new EmptyResponse();
    }
}