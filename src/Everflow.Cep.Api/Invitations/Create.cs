using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Invitations;

public class Create(IMediator mediator, FluentValidation.IValidator<CreateInvitationRequest> validator)
    : Endpoint<CreateInvitationRequest, CreateInvitationResponse>
{
    public override void Configure()
    {
        Post("/invitations");
        AllowAnonymous();
        Description(x => x
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemFE<InternalErrorResponse>(500)
            .Produces<CreateInvitationResponse>(201, "application/json"));
    }

    public override async Task HandleAsync(CreateInvitationRequest req, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid) validationResult.Errors.ForEach(AddError);
        ThrowIfAnyErrors();

        var result = await mediator.Send(req.ToCreateInvitationCommand(), ct);

        await result.Match<Task>(
            id => SendCreatedAtAsync<GetById>(
                new { id }, new CreateInvitationResponse(id), cancellation: ct),
            error => Task.FromException(new Exception(error.Description))
        );
    }
}