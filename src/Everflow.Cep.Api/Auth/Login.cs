using Everflow.Cep.Application.Auth;
using FastEndpoints;
using FastEndpoints.Security;
using FluentValidation;
using MediatR;

namespace Everflow.Cep.Api.Auth;

public class Login(IMediator mediator, IValidator<LoginRequest> validator) : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
        Description(x => x
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemFE<InternalErrorResponse>(500)
            .Produces(401)
            .Produces<LoginResponse>(200, "application/json"));
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid)validationResult.Errors.ForEach(AddError);
        ThrowIfAnyErrors();

        var result = await mediator.Send(new LoginQuery(req.Username, req.Password), ct);
        if (result.IsFailure) await SendUnauthorizedAsync(ct);
    
        var jwtToken = JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = "c73e933a293bec6f005080d0293b93e2c886ff4b9c49d4f666340c2a1170b007";
                o.ExpireAt = DateTime.UtcNow.AddDays(1);
                o.User.Claims.Add(("Username", result.Value!.Username));
                o.User.Claims.Add(("Id", result.Value!.Id.ToString()));
            });

        await SendOkAsync(new LoginResponse(result.Value!.Username, jwtToken), cancellation: ct);
    }
}