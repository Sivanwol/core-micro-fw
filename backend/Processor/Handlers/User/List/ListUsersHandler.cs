using Domain.Context;
using Infrastructure.Responses.Processor.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Processor.Handlers.User.List;

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