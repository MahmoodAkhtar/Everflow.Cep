using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Users.Delete;

public record DeleteUserByIdCommand(int Id) : IRequest<Result<bool, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("DeleteUserByIdCommand.IsNull", "DeleteUserByIdCommand is null");
    }
}

