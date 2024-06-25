using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Invitations.Delete;


public record DeleteInvitationByIdCommand(int Id) : IRequest<Result<bool, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("DeleteInvitationByIdCommand.IsNull", "DeleteInvitationByIdCommand is null");
    }
}
