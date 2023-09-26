using Application.Responses.Users;
using Domain.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Processor.Handlers.User.List; 

public class ListUsersHandler: IRequestHandler<ListUsersRequest, List<ListUserResponse>> {
    private readonly IDomainContext _context;
    private readonly IMediator _mediator;
    public ListUsersHandler(IMediator mediator, IDomainContext context)
    {
        _context = context;
        _mediator = mediator;
    }
    
    public async Task<List<ListUserResponse>> Handle(ListUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await _context.Users.ToListAsync(cancellationToken);
        return users.Select(x => new ListUserResponse {
            Id = x.Id,
            Auth0Id = x.Auth0Id
        }).ToList();
    }
}