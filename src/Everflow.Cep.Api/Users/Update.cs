using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Users;

public class Update(IMediator mediator, FluentValidation.IValidator<UpdateUserRequest> validator) 
    : Endpoint<UpdateUserRequest, UpdateUserResponse>
{
    public override void Configure()
    {
        Put("/users/{id}");
        AllowAnonymous();
        Description(x => x
                .ProducesProblemDetails(400, "application/json+problem")
                .ProducesProblemFE<InternalErrorResponse>(500)
                .Produces<UpdateUserResponse>(200, "application/json")
                .Produces(404));
    }

    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
    {
        var id = Assign.OrAction(Route<int>("id"), new User.IdSpecification(), 
            id => 
            {
                AddError(Core.Users.User.Errors.IdMustBeGreaterThanZero.Description, Core.Users.User.Errors.IdMustBeGreaterThanZero.Code);
            });
        
        var validationResult = await validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid) validationResult.Errors.ForEach(AddError);
        ThrowIfAnyErrors();

        var result = await mediator.Send(req.ToUpdateUserCommand(id), ct);

        await result.Match<Task>(
            user => SendOkAsync(new UpdateUserResponse(UserRecord.FromDto(user)), cancellation: ct),
            error => error.Type == ErrorType.NotFound 
                ? SendNotFoundAsync(ct)
                : throw new Exception(error.Description));
    }
}

