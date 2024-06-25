using Everflow.Cep.Application.Events.Delete;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Events;

public class DeleteById(IMediator mediator) : EndpointWithoutRequest<bool>
{
    public override void Configure()
    {
        Delete("/events/{id}");
        AllowAnonymous();
        Description(x => x
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemFE<InternalErrorResponse>(500)
            .Produces(204)
            .Produces(404)
            .Produces<bool>(200, "application/json"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Assign.OrAction(Route<int>("id"), new User.IdSpecification(), 
            id => 
            {
                AddError(Core.Events.Event.Errors.IdMustBeGreaterThanZero.Description, Core.Events.Event.Errors.IdMustBeGreaterThanZero.Code);
            });
        
        ThrowIfAnyErrors();

        var result = await mediator.Send(new DeleteEventByIdCommand(id), ct);

        await result.Match(
            isDeleted => isDeleted
                ? SendOkAsync(isDeleted, ct)
                : SendNotFoundAsync(ct),
            error => error.Type == ErrorType.NotFound
                ? SendNotFoundAsync(ct)
                : throw new Exception(error.Description));
    }
}