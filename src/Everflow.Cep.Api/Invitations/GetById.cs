using Everflow.Cep.Application.Invitations.Get;
using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Invitations;
using Everflow.SharedKernal;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Invitations;

public class GetById(IMediator mediator) : EndpointWithoutRequest<GetInvitationByIdResponse>
{
    public override void Configure()
    {
        Get("/invitations/{id}");
        AllowAnonymous();
        Description(x => x
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemFE<InternalErrorResponse>(500)
            .Produces<GetInvitationByIdResponse>(200, "application/json")
            .Produces(404));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Assign.OrAction(Route<int>("id"), new Event.IdSpecification(), 
            id => 
            {
                AddError(
                    Invitation.Errors.IdMustBeGreaterThanZero.Description, 
                    Invitation.Errors.IdMustBeGreaterThanZero.Code);
            });
        
        ThrowIfAnyErrors();

        var result = await mediator.Send(new GetInvitationByIdQuery(id), ct);

        await result.Match(
            invitation => invitation is null
                ? SendNotFoundAsync(ct)
                : SendAsync(new GetInvitationByIdResponse(InvitationRecord.FromDto(invitation)), 200, ct),
            error => error.Type == ErrorType.NotFound
                ? SendNotFoundAsync(ct)
                : throw new Exception(error.Description));
    }
}