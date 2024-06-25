using Everflow.Cep.Application.Users.Delete;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Users;

public class DeleteById(IMediator mediator) : EndpointWithoutRequest<bool>
{
    public override void Configure()
    {
        Delete("/users/{id}");
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
        var id = Assign.OrAction(Route<int>("id"), new User.IdSpecification(), 
            id => 
            {
                AddError(Core.Users.User.Errors.IdMustBeGreaterThanZero.Description, Core.Users.User.Errors.IdMustBeGreaterThanZero.Code);
            });
        
        ThrowIfAnyErrors();

        var result = await mediator.Send(new DeleteUserByIdCommand(id), ct);

        await result.Match(
            isDeleted => isDeleted
                ? SendOkAsync(isDeleted, ct)
                : SendNotFoundAsync(ct),
            error => error.Type == ErrorType.NotFound
                ? SendNotFoundAsync(ct)
                : throw new Exception(error.Description));
    }
}