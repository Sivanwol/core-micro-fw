using Infrastructure.Responses.Common;
using MediatR;
namespace Processor.Services.User.Create;

public class CreateUserRequest : IRequest<InsertIdResponse> {
    public string Auth0Id { get; set; }
}