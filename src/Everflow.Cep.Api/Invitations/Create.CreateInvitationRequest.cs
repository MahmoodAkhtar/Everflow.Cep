using Everflow.Cep.Application.Invitations;
using Everflow.Cep.Application.Invitations.Create;

namespace Everflow.Cep.Api.Invitations;

public class CreateInvitationRequest
{
    public int InvitedUserId { get; set; }
    public int InvitedToEventId { get; set; }
    public DateTime SentDateTime { get; set; }
    public string ResponseStatus { get; set; }
}

public static class CreateInvitationRequestExtensions
{
    public static CreateInvitationCommand ToCreateInvitationCommand(this CreateInvitationRequest request)
        => new(
            request.InvitedUserId,
            request.InvitedToEventId,
            request.SentDateTime,
            InvitationResponseStatusDtos.FromString(request.ResponseStatus));
}