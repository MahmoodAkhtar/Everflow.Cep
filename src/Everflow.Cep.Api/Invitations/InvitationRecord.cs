using Everflow.Cep.Api.Events;
using Everflow.Cep.Api.Users;
using Everflow.Cep.Application.Invitations;

namespace Everflow.Cep.Api.Invitations;

public record InvitationRecord(
    int Id,
    UserRecord InvitedUser,
    EventRecord InvitedToEvent,
    DateTime SentDateTime,
    string ResponseStatus)
{
    public static InvitationRecord FromDto(InvitationDto invitation)
        => new(
            invitation.Id,
            UserRecord.FromDto(invitation.InvitedUser),
            EventRecord.FromDto(invitation.InvitedToEvent),
            invitation.SentDateTime,
            InvitationResponseStatusRecord.FromDto(invitation.ResponseStatus).ToString());
}