using Infrastructure.Responses.Processor.Users;
using MediatR;

namespace Processor.Handlers.User.List;

public class ListUsersRequest : IRequest<List<ListUserResponse>> { }