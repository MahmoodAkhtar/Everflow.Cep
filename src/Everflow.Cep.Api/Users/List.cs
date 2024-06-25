using Everflow.Cep.Application.Users.List;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Users;

public class List(IMediator mediator, FluentValidation.IValidator<ListUsersRequest> validator)
    : Endpoint<ListUsersRequest, ListUsersResponse>
{
    public override void Configure()
    {
        Get("/users/{limit}/{offset}");
        AllowAnonymous();
        Description(x => x
                .ProducesProblemDetails(400, "application/json+problem")
                .ProducesProblemFE<InternalErrorResponse>(500)
                .Produces<ListUsersResponse>(200, "application/json"));
    }

    public override async Task HandleAsync(ListUsersRequest req, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid) validationResult.Errors.ForEach(AddError);
        ThrowIfAnyErrors();

        var result = await mediator.Send(new ListUsersQuery(req.Offset, req.Limit), ct);

        await result.Match(
            users => SendAsync(new ListUsersResponse(users.Select(UserRecord.FromDto)), 200, ct),
            error => throw new Exception(error.Description));
    }
}