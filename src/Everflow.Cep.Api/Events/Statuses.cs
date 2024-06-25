using Everflow.Cep.Application.Events.List;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Events;

public class Statuses(IMediator mediator)
    : EndpointWithoutRequest<ListEventStatusesResponse>
{
    public override void Configure()
    {
        Get("/events/statuses");
        AllowAnonymous();
        Description(x => x
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemFE<InternalErrorResponse>(500)
            .Produces<ListEventStatusesResponse>(200, "application/json"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new ListEventStatusesQuery(), ct);

        await result.Match(
            events => SendAsync(
                new ListEventStatusesResponse(events
                    .Select(EventStatusRecord.FromDto)
                    .Select(x => x.ToString())), 200, ct),
            error => throw new Exception(error.Description));
    }
}