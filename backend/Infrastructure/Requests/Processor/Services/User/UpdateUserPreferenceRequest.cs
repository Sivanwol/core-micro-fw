using Infrastructure.GQL.Inputs.User;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class UpdateUserPreferenceRequest : BaseRequest<bool> {
    public IList<UserPreferenceInput> Preferences { get; set; }
}