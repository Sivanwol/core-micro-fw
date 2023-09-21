using Application.Exceptions;
using Application.Responses.Common;
using Application.Utils;
using AutoMapper;
using Domain.Context;
using MassTransit;
using MediatR;
using Processor.Consumers.IndexUser;

namespace Processor.Handlers.User.Create; 

public class CreateUserHandler: IRequestHandler<CreateUserRequest, InsertIdResponse> {
    private readonly DomainContext _context;
    private readonly IPublishEndpoint _bus;
    private readonly IMediator _mediator;
    public CreateUserHandler(IMediator mediator,  DomainContext context,IPublishEndpoint publish)
    {
        _bus = publish;
        _context = context;
        _mediator = mediator;
    }
    
    public async Task<InsertIdResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FirstOrDefault(x => x.Auth0Id == request.Auth0Id && x.Active == true);
        if (user != null) throw new EntityFoundException("users", user.Id.ToString());
        var entity = new Domain.Entities.User() {
            Auth0Id = request.Auth0Id,
            Active = true
        };
        _context.Users.Add(entity);
        _context.SaveChanges();
        await _bus.Publish(new IndexUserEvent {
            User = entity
        }, cancellationToken);
        return new InsertIdResponse {
            EntityId = entity.Id
        };;
    }
}