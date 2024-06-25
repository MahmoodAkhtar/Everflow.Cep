using Everflow.Cep.Application.Users.Create;

namespace Everflow.Cep.Api.Users;

public class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public static class CreateUserRequestExtensions
{
    public static CreateUserCommand ToCreateUserCommand(this CreateUserRequest request)
        => new(request.Name, request.Email, request.Password);
}