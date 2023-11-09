using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.General;
using Infrastructure.Responses.Controllers;
using MediatR;
using Microsoft.Extensions.Logging;
namespace Processor.Services.General;

public class GetApplicationSettingsHandler : IRequestHandler<GetApplicationSettingsRequest, GeneralResponse> {
    private readonly ICountriesRepository _countriesRepository;
    private readonly IEthnicitiesRepository _ethnicitiesRepository;
    private readonly ILanguagesRepository _languagesRepository;
    private readonly ILogger _logger;
    private readonly IReligionsRepository _religionsRepository;
    public GetApplicationSettingsHandler(
        ILoggerFactory loggerFactory,
        ICountriesRepository countriesRepositorry,
        IEthnicitiesRepository ethnicitiesRepository,
        ILanguagesRepository languagesRepository,
        IReligionsRepository religionsRepository) {
        _countriesRepository = countriesRepositorry;
        _ethnicitiesRepository = ethnicitiesRepository;
        _languagesRepository = languagesRepository;
        _religionsRepository = religionsRepository;
        _logger = loggerFactory.CreateLogger<GetApplicationSettingsHandler>();
    }


    public async Task<GeneralResponse> Handle(GetApplicationSettingsRequest request,
        CancellationToken cancellationToken) {
        var languages = await _languagesRepository.GetAll();
        var ethnicities = await _ethnicitiesRepository.GetAll();
        var religions = await _religionsRepository.GetAll();
        var countries = await _countriesRepository.GetAll();
        _logger.LogInformation("Process GetApplicationSettingsHandler finished");
        return new GeneralResponse {
            Languages = languages.Select(s => new LanguagesResponse {
                ID = s.Id,
                Name = s.Name,
                Code = s.Code
            }).ToList(),
            Religions = religions.Select(s => new ReligionsResponse {
                ID = s.Id,
                Name = s.Name
            }).ToList(),
            Ethnicities = ethnicities.Select(s => new EthnicitiesResponse {
                ID = s.Id,
                Name = s.Name
            }).ToList(),
            Countries = countries.Select(s => new CountriesResponse {
                ID = s.Id,
                CountryName = s.CountryName,
                CountryCode = s.CountryCode,
                CountryCode3 = s.CountryCode,
                CountryNumber = s.CountryNumber
            }).ToList()
        };
    }
}