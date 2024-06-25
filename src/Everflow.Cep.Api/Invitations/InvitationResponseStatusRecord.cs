using Everflow.Cep.Application.Invitations;

namespace Everflow.Cep.Api.Invitations;

public abstract record InvitationResponseStatusRecord
{
    public static InvitationResponseStatusRecord FromDto(InvitationResponseStatusDto status)
    {
        if (InvitationResponseStatusDtos.NoReply.GetType() == status.GetType()) return InvitationResponseStatusRecords.NoReply;
        if (InvitationResponseStatusDtos.Accept.GetType() == status.GetType()) return InvitationResponseStatusRecords.Accept;
        if (InvitationResponseStatusDtos.Reject.GetType() == status.GetType()) return InvitationResponseStatusRecords.Reject;
        if (InvitationResponseStatusDtos.Maybe.GetType() == status.GetType()) return InvitationResponseStatusRecords.Maybe;
        return InvitationResponseStatusRecords.NoReply;
    }
    
    public sealed override string ToString()
        => GetType().Name;
}

public record NoReply : InvitationResponseStatusRecord;

public record Accept : InvitationResponseStatusRecord;

public record Reject : InvitationResponseStatusRecord;

public record Maybe : InvitationResponseStatusRecord;

public static class InvitationResponseStatusRecords
{
    public static readonly NoReply NoReply = new NoReply();
    public static readonly Accept Accept = new Accept();
    public static readonly Reject Reject = new Reject();
    public static readonly Maybe Maybe = new Maybe();

    public static InvitationResponseStatusRecord FromString(string value)
        => (InvitationResponseStatusRecord)Activator
            .CreateInstanceFrom(
                typeof(InvitationResponseStatusRecord).Assembly.FullName, 
                $"{typeof(InvitationResponseStatusRecord).FullName!
                    .Remove(typeof(InvitationResponseStatusRecord).FullName!
                        .LastIndexOf('.'))}.{value}")
            .Unwrap();
}