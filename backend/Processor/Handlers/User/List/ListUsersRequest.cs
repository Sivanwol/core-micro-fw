using Application.Responses.Users;
using MediatR;

namespace Processor.Handlers.User.List; 

public class ListUsersRequest : IRequest<List<ListUserResponse>> {
    
}