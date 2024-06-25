using Everflow.Cep.Application.Users.Get;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;
using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Users;

public class GetById(IMediator mediator) : EndpointWithoutRequest<GetUserByIdResponse>
{
    public override void Configure()
    {
        Get("/users/{id}");
        AllowAnonymous();
        Description(x => x
                .ProducesProblemDetails(400, "application/json+problem")
                .ProducesProblemFE<InternalErrorResponse>(500)
                .Produces<GetUserByIdResponse>(200, "application/json")
                .Produces(404));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Assign.OrAction(Route<int>("id"), new User.IdSpecification(), 
            id => 
            {
                AddError(Core.Users.User.Errors.IdMustBeGreaterThanZero.Description, Core.Users.User.Errors.IdMustBeGreaterThanZero.Code);
            });
        
        ThrowIfAnyErrors();

        var result = await mediator.Send(new GetUserByIdQuery(id), ct);

        await result.Match(
            user => user is null
                ? SendNotFoundAsync(ct)
                : SendAsync(new GetUserByIdResponse(UserRecord.FromDto(user)), 200, ct),
            error => error.Type == ErrorType.NotFound
                ? SendNotFoundAsync(ct)
                : throw new Exception(error.Description));
    }
}