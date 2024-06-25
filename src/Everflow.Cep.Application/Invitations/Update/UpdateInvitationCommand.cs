using Everflow.Cep.Application.Events;
using Everflow.Cep.Application.Users;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Invitations.Update;

public record UpdateInvitationCommand(
    int Id,
    int InvitedUserId,
    int InvitedToEventId,
    DateTime SentDateTime,
    InvitationResponseStatusDto ResponseStatus) : IRequest<Result<InvitationDto, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict(
            "UpdateInvitationCommand.IsNull", 
            "UpdateInvitationCommand is null");
    }
}