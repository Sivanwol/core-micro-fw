using Application.Configs;
using Application.Constraints;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Filters;
using Domain.Filters.Fields;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.GQL.Common;
using Infrastructure.Requests.Processor.Services.Client;
using Infrastructure.Services.Cache;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Serilog;
namespace Processor.Services.Client;

public class HasClientExistHandler : IRequestHandler<HasClientExistRequest, bool> {
    private readonly ICountriesRepository _countriesRepository;
    private readonly IClientRepository _clientRepository;
    public HasClientExistHandler(
        ICountriesRepository countriesRepository,
        IClientRepository clientRepository) {
        _countriesRepository = countriesRepository;
        _clientRepository = clientRepository;
    }
    public async Task<bool> Handle(HasClientExistRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"HasClientExistHandler: been trigger");
        if (_countriesRepository.GetById(request.CountryId) == null) {
            throw new EntityNotFoundException("Country", request.CountryId);
        }
        return await _clientRepository.HasClientExist(request.Name, request.CountryId);
    }
}