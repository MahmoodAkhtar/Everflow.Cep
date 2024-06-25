using Everflow.Cep.Core.Invitations;

namespace Everflow.Cep.Application.Invitations;

public abstract record InvitationResponseStatusDto
{
    public static InvitationResponseStatusDto FromInvitationResponseStatus(InvitationResponseStatus status)
    {
        if (InvitationResponseStatuses.NoReply.GetType() == status.GetType()) return InvitationResponseStatusDtos.NoReply;
        if (InvitationResponseStatuses.Accept.GetType() == status.GetType()) return InvitationResponseStatusDtos.Accept;
        if (InvitationResponseStatuses.Reject.GetType() == status.GetType()) return InvitationResponseStatusDtos.Reject;
        if (InvitationResponseStatuses.Maybe.GetType() == status.GetType()) return InvitationResponseStatusDtos.Maybe;
        return InvitationResponseStatusDtos.NoReply;
    }
    
    public sealed override string ToString()
        => GetType().Name;
}

public record NoReply : InvitationResponseStatusDto;

public record Accept : InvitationResponseStatusDto;

public record Reject : InvitationResponseStatusDto;

public record Maybe : InvitationResponseStatusDto;

public static class InvitationResponseStatusDtos
{
    public static readonly NoReply NoReply = new NoReply();
    public static readonly Accept Accept = new Accept();
    public static readonly Reject Reject = new Reject();
    public static readonly Maybe Maybe = new Maybe();

    public static InvitationResponseStatusDto FromString(string value)
        => (InvitationResponseStatusDto)Activator
            .CreateInstance(
                typeof(InvitationResponseStatusDto).Assembly.FullName, 
                $"{typeof(InvitationResponseStatusDto).FullName!
                    .Remove(typeof(InvitationResponseStatusDto).FullName!.LastIndexOf('.'))}.{value}")
            .Unwrap();
}