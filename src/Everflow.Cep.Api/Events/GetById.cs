using Everflow.Cep.Application.Events.Get;
using Everflow.Cep.Core.Events;
using Everflow.SharedKernal;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Events;

public class GetById(IMediator mediator) : EndpointWithoutRequest<GetEventByIdResponse>
{
    public override void Configure()
    {
        Get("/events/{id}");
        AllowAnonymous();
        Description(x => x
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemFE<InternalErrorResponse>(500)
            .Produces<GetEventByIdResponse>(200, "application/json")
            .Produces(404));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Assign.OrAction(Route<int>("id"), new Event.IdSpecification(), 
            id => 
            {
                AddError(Event.Errors.IdMustBeGreaterThanZero.Description, Event.Errors.IdMustBeGreaterThanZero.Code);
            });
        
        ThrowIfAnyErrors();

        var result = await mediator.Send(new GetEventByIdQuery(id), ct);

        await result.Match(
            @event => @event is null
                ? SendNotFoundAsync(ct)
                : SendAsync(new GetEventByIdResponse(EventRecord.FromDto(@event)), 200, ct),
            error => error.Type == ErrorType.NotFound
                ? SendNotFoundAsync(ct)
                : throw new Exception(error.Description));
    }
}