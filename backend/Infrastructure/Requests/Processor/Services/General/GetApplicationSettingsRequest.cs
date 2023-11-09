using Infrastructure.Responses.Controllers;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.General;

public class GetApplicationSettingsRequest : IRequest<GeneralResponse> { }