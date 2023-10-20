using Infrastructure.Responses.Processor.Services.Users;
using MediatR;
namespace Processor.Services.User.List;

public class ListUsersRequest : IRequest<List<ListUserResponse>> { }