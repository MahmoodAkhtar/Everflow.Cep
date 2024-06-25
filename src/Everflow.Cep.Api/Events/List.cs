using Everflow.Cep.Application.Events.List;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Events;

public class List(IMediator mediator, FluentValidation.IValidator<ListEventsRequest> validator)
    : Endpoint<ListEventsRequest, ListEventsResponse>
{
    public override void Configure()
    {
        Get("/events/{limit}/{offset}");
        //AllowAnonymous();
        Description(x => x
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemFE<InternalErrorResponse>(500)
            .Produces<ListEventsResponse>(200, "application/json"));
    }

    public override async Task HandleAsync(ListEventsRequest req, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid) validationResult.Errors.ForEach(AddError);
        ThrowIfAnyErrors();

        var result = await mediator.Send(new ListEventsQuery(req.Offset, req.Limit), ct);

        await result.Match(
            events => SendAsync(new ListEventsResponse(
                events.Select(EventRecord.FromDto)), 200, ct),
            error => throw new Exception(error.Description));
    }
}