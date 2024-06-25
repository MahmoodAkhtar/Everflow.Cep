using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Events.Delete;

public record DeleteEventByIdCommand(int Id) : IRequest<Result<bool, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("DeleteEventByIdCommand.IsNull", "DeleteEventByIdCommand is null");
    }
}


