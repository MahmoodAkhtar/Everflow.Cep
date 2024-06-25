using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Invitations.Create;

public record CreateInvitationCommand(
    int InvitedUserId,
    int InvitedToEventId,
    DateTime SentDateTime,
    InvitationResponseStatusDto ResponseStatus) : IRequest<Result<int, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict(
            "CreateInvitationCommand.IsNull", 
            "CreateInvitationCommand is null");
    }
}