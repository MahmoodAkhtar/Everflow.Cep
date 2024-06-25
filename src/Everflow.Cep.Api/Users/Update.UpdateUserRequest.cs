using Everflow.Cep.Application.Users.Update;

namespace Everflow.Cep.Api.Users;

public class UpdateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public static class UpdateUserRequestExtensions
{
    public static UpdateUserCommand ToUpdateUserCommand(this UpdateUserRequest request, int id)
        => new(id, request.Name, request.Email, request.Password);
}