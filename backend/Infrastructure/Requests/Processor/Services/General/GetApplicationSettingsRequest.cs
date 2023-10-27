using Infrastructure.Responses.App;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.General;

public class GetApplicationSettingsRequest : IRequest<GeneralResponse> { }