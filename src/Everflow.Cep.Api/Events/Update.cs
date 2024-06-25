using Everflow.Cep.Core.Events;
using Everflow.SharedKernal;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Events;

public class Update(IMediator mediator, FluentValidation.IValidator<UpdateEventRequest> validator) 
    : Endpoint<UpdateEventRequest, UpdateEventResponse>
{
    public override void Configure()
    {
        Put("/events/{id}");
        AllowAnonymous();
        Description(x => x
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemFE<InternalErrorResponse>(500)
            .Produces<UpdateEventResponse>(200, "application/json")
            .Produces(404));
    }

    public override async Task HandleAsync(UpdateEventRequest req, CancellationToken ct)
    {
        var id = Assign.OrAction(Route<int>("id"), new Event.IdSpecification(), 
            id => 
            {
                AddError(Event.Errors.IdMustBeGreaterThanZero.Description, Event.Errors.IdMustBeGreaterThanZero.Code);
            });
        
        var validationResult = await validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid) validationResult.Errors.ForEach(AddError);
        ThrowIfAnyErrors();

        var result = await mediator.Send(req.ToUpdateEventCommand(id), ct);

        await result.Match<Task>(
            @event => SendOkAsync(new UpdateEventResponse(EventRecord.FromDto(@event)), ct),
            error => error.Type == ErrorType.NotFound 
                ? SendNotFoundAsync(ct)
                : throw new Exception(error.Description));
    }
}