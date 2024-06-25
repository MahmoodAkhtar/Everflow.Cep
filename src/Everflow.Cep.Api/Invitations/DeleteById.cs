using Everflow.Cep.Application.Invitations.Delete;
using Everflow.Cep.Core.Invitations;
using Everflow.SharedKernal;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Invitations;

public class DeleteById(IMediator mediator) : EndpointWithoutRequest<bool>
{
    public override void Configure()
    {
        Delete("/invitations/{id}");
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
        var id = Assign.OrAction(Route<int>("id"), new Invitation.IdSpecification(),
            id =>
            {
                AddError(
                    Invitation.Errors.IdMustBeGreaterThanZero.Description,
                    Invitation.Errors.IdMustBeGreaterThanZero.Code);
            });

        ThrowIfAnyErrors();

        var result = await mediator.Send(new DeleteInvitationByIdCommand(id), ct);

        await result.Match(
            isDeleted => isDeleted
                ? SendOkAsync(isDeleted, ct)
                : SendNotFoundAsync(ct),
            error => error.Type == ErrorType.NotFound
                ? SendNotFoundAsync(ct)
                : throw new Exception(error.Description));
    }
}