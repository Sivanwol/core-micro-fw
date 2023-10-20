using Application.Exceptions;
using Domain.Persistence.Context;
using Infrastructure.Responses.Common;
using MassTransit;
using MediatR;
using Processor.Consumers.IndexUser;
namespace Processor.Services.User.Create;

public class CreateUserHandler : IRequestHandler<CreateUserRequest, InsertIdResponse> {
    private readonly IPublishEndpoint _bus;
    private readonly IDomainContext _context;
    private readonly IMediator _mediator;

    public CreateUserHandler(IMediator mediator, IDomainContext context, IPublishEndpoint publish) {
        _bus = publish;
        _context = context;
        _mediator = mediator;
    }

    public async Task<InsertIdResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken) {
        var user = _context;
        if (user != null) throw new EntityFoundException("users", user.ToString());
        // var entity = new Domain.Entities.User() { };
        _context.Instance.SaveChanges();
        await _bus.Publish(new IndexUserEvent {
            // User = entity
        }, cancellationToken);
        return new InsertIdResponse { };
        ;
    }
}