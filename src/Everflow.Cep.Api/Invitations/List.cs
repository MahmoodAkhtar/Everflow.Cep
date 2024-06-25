using Everflow.Cep.Api.Events;
using Everflow.Cep.Application.Invitations.List;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Invitations;

public class List(IMediator mediator, FluentValidation.IValidator<ListInvitationsRequest> validator)
    : Endpoint<ListInvitationsRequest, ListInvitationsResponse>
{
    public override void Configure()
    {
        Get("/invitations/{limit}/{offset}");
        AllowAnonymous();
        Description(x => x
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemFE<InternalErrorResponse>(500)
            .Produces<ListEventsResponse>(200, "application/json"));
    }

    public override async Task HandleAsync(ListInvitationsRequest req, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid) validationResult.Errors.ForEach(AddError);
        ThrowIfAnyErrors();

        var result = await mediator.Send(new ListInvitationsQuery(req.Offset, req.Limit), ct);

        await result.Match(
            invitations => SendAsync(new ListInvitationsResponse(
                invitations.Select(InvitationRecord.FromDto)), 200, ct),
            error => throw new Exception(error.Description));
    }
}