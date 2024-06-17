using Application.Configs;
using Application.Constraints;
using Application.Contract;
using Application.Exceptions;
using Application.Responses.Base;
using Application.Utils;
using Domain.DTO.User;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Domain.Utils;
using EasyCaching.Core;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Services.Email;
using Infrastructure.Services.S3;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Serilog;
namespace Processor.Services.User;

public class RegisterHandler : IRequestHandler<RegisterRequest, EmptyResponse> {
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    
    private readonly IStorageControlService _storageControlService;
    private readonly IRedisCachingProvider _cache;
    private readonly BackendApplicationConfig _config;
    private readonly ICountriesRepository _countriesRepository;
    private readonly IMailService _emailService;
    private readonly ILanguagesRepository _languagesRepository;
    private readonly IMediator _mediator;
    private readonly ITemplateService _templateService;
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterHandler(IMediator mediator,
        UserManager<ApplicationUser> userManager,
        IEasyCachingProviderFactory factory,
        BackendApplicationConfig config,
        IApplicationUserRepository applicationUserRepository,
        ICountriesRepository countriesRepository,
        ILanguagesRepository languagesRepository,
        ITemplateService templateService,
        IStorageControlService storageControlService,
        IMailService emailService) {
        _mediator = mediator;
        _cache = factory.GetRedisProvider(Cache.ProviderName);
        _userManager = userManager;
        _applicationUserRepository = applicationUserRepository;
        _templateService = templateService;
        _countriesRepository = countriesRepository;
        _languagesRepository = languagesRepository;
        _emailService = emailService;
        _storageControlService = storageControlService;
        _config = config;
    }

    public async Task<EmptyResponse> Handle(RegisterRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"RegisterHandler: Start user register [{request.Email}]");
        if (request.CountryId == null && string.IsNullOrEmpty(request.PhoneNumber)) {
            Log.Logger.Warning("CountryId is null");
            throw new InvalidRequestException("Phone number is required");
        }
        var loggedUser = await _userManager.FindByIdAsync(request.OwnerUserId.ToString());
        if (loggedUser == null) {
            Log.Logger.Warning("Owner User not found");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.OwnerUserId.ToString());
        }

        var hasMatchingRoles = await AuthUtils.HasMathchingRoles(_userManager, loggedUser, [Roles.Admin, Roles.IT]);
        if (!hasMatchingRoles) {
            Log.Logger.Warning("Invalid owner user not matching permissions");
            await _activityLogRepository.AddActivity(
                Guid.Parse(loggedUser.Id),
                loggedUser.Id,
                nameof(ApplicationUser),
                ActivityLogOperationType.UserRegister,
                "User Request Registered", "User request registered owner User not matching permissions",
                ActivityStatus.Failed);
            throw new AuthenticationException("Invalid user not matching permissions");
        }
        var cacheKey = await _cache.KeyExistsAsync(Cache.GetKey($"COUNTRY:{request.CountryId}"));
        Domain.Entities.Countries? country = null;
        if (cacheKey) {
            var payload = await _cache.StringGetAsync(Cache.GetKey($"COUNTRY:{request.CountryId}"));
            country = JsonConvert.DeserializeObject<Domain.Entities.Countries>(payload);
        }
        if (country == null) {
            country = await _countriesRepository.GetById(request.CountryId!.Value);
            if (country == null) {
                Log.Logger.Warning("Country not found");
                throw new EntityNotFoundException(nameof(Countries), request.CountryId.Value);
            }
            await _cache.StringSetAsync(Cache.GetKey($"COUNTRY_{request.CountryId}"), country.ToJson(), TimeSpan.FromDays(1));
        }
        if (country == null) {
            Log.Logger.Warning("Country not found");
            throw new EntityNotFoundException(nameof(Countries), request.CountryId.Value);
        }
        var language = await _languagesRepository.GetById(request.DisplayLanguageId);
        if (language == null) {
            Log.Logger.Warning("Language not found");
            throw new EntityNotFoundException(nameof(Languages), request.DisplayLanguageId);
        }
        var userExists = await _applicationUserRepository.LocateUserByEmail(request.Email);
        if (userExists != null) {
            Log.Logger.Warning("User already exists");
            throw new EntityFoundException(nameof(ApplicationUser), request.Email);
        }
        var user = new ApplicationUser {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Country = country,
            DisplayLanguage = language,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded) {
            await _userManager.AddToRoleAsync(user, Roles.Client);
            Log.Logger.Information($"RegisterHandler: User {user.Id} has storage register");
            await _storageControlService.CreateFolder(StorageAws.BucketUploadBins,$"users/{user.Id}");
            await _storageControlService.CreateFolder(StorageAws.BucketUserMedia,user.Id);
            var otpEntity = await _applicationUserRepository.GenerateRegistrationOtpCode(user);
            Log.Logger.Information($"RegisterHandler: User {user.Id} has been created and otp entity id {otpEntity.Id}");
            var emailTemplate = await _templateService.GetTemplateHtmlAsStringAsync(EmailTemplates.UserRegistration, new RegisterMail {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailConfirmationLink = $"{request.BaseUrl}/api/v1/Auth/confirm-email?userToken={user.Token}&otpToken={otpEntity.Token}&code={otpEntity.Code}",
                HomePageUrl = request.BaseUrl,
                HostImageUrl = $"{request.BaseUrl}/assets",

            });

            var mail = new MailData(new List<string> {
                user.Email!
            }, "Welcome To the platform", emailTemplate, _config.EmailFrom, _config.EmailFromName);
            mail.Bcc.Add(_config.ShadowEmailBcc);

            await _emailService.SendAsync(mail, cancellationToken);
            await _activityLogRepository.AddActivity(
                Guid.Parse(user.Id),
                Guid.Parse(loggedUser.Id),
                user.Id,
                nameof(ApplicationUser),
                ActivityLogOperationType.UserRegister,
                "User Request Registered", "User request registered",
                ActivityStatus.Success);
            return new EmptyResponse();
        }
        throw new Exception(result.Errors.First().Description);
    }
}