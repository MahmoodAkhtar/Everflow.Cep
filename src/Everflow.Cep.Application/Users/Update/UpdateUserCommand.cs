using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Users.Update;

public record UpdateUserCommand(int Id, string Name, string Email, string Password) : IRequest<Result<UserDto, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("UpdateUserCommand.IsNull", "UpdateUserCommand is null");
        
    }
}