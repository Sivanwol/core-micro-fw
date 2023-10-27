using System.Runtime.Intrinsics.X86;
using MassTransit;
using Domain.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.General;
using Infrastructure.Responses.App;
using MediatR;
using Microsoft.Extensions.Logging;
namespace Processor.Services.General;

public class GetApplicationSettingsHandler : IRequestHandler<GetApplicationSettingsRequest, GeneralResponse> {
    private readonly ILogger _logger;
    private readonly ICountriesRepository _countriesRepository;
    private readonly IEthnicitiesRepository _ethnicitiesRepository;
    private readonly ILanguagesRepository _languagesRepository;
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
            Languages = languages.Select(s => new LanguagesResponse {ID= s.ID, Name= s.Name, Code= s.Code}).ToList(),
            Religions = religions.Select(s => new ReligionsResponse {ID=  s.ID, Name= s.Name}).ToList(),
            Ethnicities = ethnicities.Select(s => new EthnicitiesResponse {ID=  s.ID, Name= s.Name}).ToList(),
            Countries = countries.Select(s => new CountriesResponse {ID= s.ID, CountryName= s.CountryName, CountryCode= s.CountryCode, CountryCode3= s.CountryCode, CountryNumber= s.CountryNumber}).ToList()
        };
    }
}