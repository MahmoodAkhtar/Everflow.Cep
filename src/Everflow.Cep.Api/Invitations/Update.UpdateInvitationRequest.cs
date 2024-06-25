using Everflow.Cep.Application.Invitations;
using Everflow.Cep.Application.Invitations.Update;

namespace Everflow.Cep.Api.Invitations;

public class UpdateInvitationRequest
{
    public int InvitedUserId { get; set; }
    public int InvitedToEventId { get; set; }
    public DateTime SentDateTime { get; set; }
    public string ResponseStatus { get; set; }
}

public static class UpdateInvitationRequestExtensions
{
    public static UpdateInvitationCommand ToUpdateInvitationCommand(this UpdateInvitationRequest request, int id)
        => new(
            id,
            request.InvitedUserId,
            request.InvitedToEventId,
            request.SentDateTime,
            InvitationResponseStatusDtos.FromString(request.ResponseStatus));
}