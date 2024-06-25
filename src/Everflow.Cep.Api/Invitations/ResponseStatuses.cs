using Everflow.Cep.Application.Invitations.List;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Invitations;

public class ResponseStatuses(IMediator mediator)
    : EndpointWithoutRequest<ListInvitationResponseStatusesResponse>
{
    public override void Configure()
    {
        Get("/invitations/response-statuses");
        AllowAnonymous();
        Description(x => x
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemFE<InternalErrorResponse>(500)
            .Produces<ListInvitationResponseStatusesResponse>(200, "application/json"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new InvitationResponseStatusesQuery(), ct);

        await result.Match(
            events => SendAsync(
                new ListInvitationResponseStatusesResponse(events
                    .Select(InvitationResponseStatusRecord.FromDto)
                    .Select(x => x.ToString())), 200, ct),
            error => throw new Exception(error.Description));
    }
}