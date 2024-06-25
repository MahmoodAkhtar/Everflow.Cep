using Everflow.Cep.Core.Invitations;
using Everflow.SharedKernal;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Invitations;

public class Update(IMediator mediator, FluentValidation.IValidator<UpdateInvitationRequest> validator) 
    : Endpoint<UpdateInvitationRequest, UpdateInvitationResponse>
{
    public override void Configure()
    {
        Put("/invitations/{id}");
        AllowAnonymous();
        Description(x => x
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemFE<InternalErrorResponse>(500)
            .Produces<UpdateInvitationResponse>(200, "application/json")
            .Produces(404));
    }

    public override async Task HandleAsync(UpdateInvitationRequest req, CancellationToken ct)
    {
        var id = Assign.OrAction(Route<int>("id"), new Invitation.IdSpecification(), 
            id => 
            {
                AddError(
                    Invitation.Errors.IdMustBeGreaterThanZero.Description, 
                    Invitation.Errors.IdMustBeGreaterThanZero.Code);
            });
        
        var validationResult = await validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid) validationResult.Errors.ForEach(AddError);
        ThrowIfAnyErrors();

        var result = await mediator.Send(req.ToUpdateInvitationCommand(id), ct);

        await result.Match<Task>(
            invitation => SendOkAsync(new UpdateInvitationResponse(InvitationRecord.FromDto(invitation)), ct),
            error => error.Type == ErrorType.NotFound 
                ? SendNotFoundAsync(ct)
                : throw new Exception(error.Description));
    }
}