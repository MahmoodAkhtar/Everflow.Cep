using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Users;

public class Create(IMediator mediator, FluentValidation.IValidator<CreateUserRequest> validator)
    : Endpoint<CreateUserRequest, CreateUserResponse>
{
    public override void Configure()
    {
        Post("/users");
        AllowAnonymous();
        Description(x => x
                .ProducesProblemDetails(400, "application/json+problem")
                .ProducesProblemFE<InternalErrorResponse>(500)
                .Produces<CreateUserResponse>(201, "application/json"));
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid) validationResult.Errors.ForEach(AddError);
        ThrowIfAnyErrors();

        var result = await mediator.Send(req.ToCreateUserCommand(), ct);

        await result.Match<Task>(
            id => SendCreatedAtAsync<GetById>(new { id }, new CreateUserResponse(id), cancellation: ct),
            error => Task.FromException(new Exception(error.Description))
        );
    }
}