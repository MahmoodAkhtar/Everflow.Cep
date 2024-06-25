using Everflow.Cep.Application.Events;
using Everflow.Cep.Application.Users;
using Everflow.Cep.Core.Invitations;

namespace Everflow.Cep.Application.Invitations;

public record InvitationDto(
    int Id,
    UserDto InvitedUser,
    EventDto InvitedToEvent,
    DateTime SentDateTime,
    InvitationResponseStatusDto ResponseStatus)
{
    public static InvitationDto FromInvitation(Invitation invitation)
        => new (
            invitation.Id,
            UserDto.FromUser(invitation.InvitedUser), 
            EventDto.FromEvent(invitation.InvitedToEvent), 
            invitation.SentDateTime, 
            InvitationResponseStatusDto.FromInvitationResponseStatus(invitation.ResponseStatus));
}