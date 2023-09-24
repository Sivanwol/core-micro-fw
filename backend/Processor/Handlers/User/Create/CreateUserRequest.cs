using Application.Responses.Common;
using MediatR;

namespace Processor.Handlers.User.Create; 

public class CreateUserRequest: IRequest<InsertIdResponse> {
    public string Auth0Id { get; set; }
}