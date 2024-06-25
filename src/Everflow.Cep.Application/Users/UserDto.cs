using Everflow.Cep.Core.Users;

namespace Everflow.Cep.Application.Users;

public record UserDto(
    int Id, 
    string Name, 
    string Email, 
    string Password)
{
    public static UserDto FromUser(User user)
        => new(
            user.Id, 
            user.Name, 
            user.Email, 
            user.Password);
}
