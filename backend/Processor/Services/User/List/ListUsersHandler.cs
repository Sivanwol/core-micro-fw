using Domain.Persistence.Context;
using Infrastructure.Responses.Processor.Services.Users;
using MediatR;
namespace Processor.Services.User.List;

public class ListUsersHandler : IRequestHandler<ListUsersRequest, List<ListUserResponse>> {
    private readonly IDomainContext _context;
    private readonly IMediator _mediator;

    public ListUsersHandler(IMediator mediator, IDomainContext context) {
        _context = context;
        _mediator = mediator;
    }

    public async Task<List<ListUserResponse>> Handle(ListUsersRequest request, CancellationToken cancellationToken) {
        return new List<ListUserResponse>();
    }
}