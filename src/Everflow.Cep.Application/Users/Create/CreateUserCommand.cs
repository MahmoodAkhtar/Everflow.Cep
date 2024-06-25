using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Users.Create;

public record CreateUserCommand(string Name, string Email, string Password) : IRequest<Result<int, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("CreateUserCommand.IsNull", "CreateUserCommand is null");
    }
}