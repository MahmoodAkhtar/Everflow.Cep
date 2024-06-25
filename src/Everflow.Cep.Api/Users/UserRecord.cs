using Everflow.Cep.Application.Users;

namespace Everflow.Cep.Api.Users;

public record UserRecord(
    int Id, 
    string Name, 
    string Email, 
    string Password)
{
    public static UserRecord FromDto(UserDto user)
        => new(
            user.Id, 
            user.Name, 
            user.Email, 
            user.Password);
}