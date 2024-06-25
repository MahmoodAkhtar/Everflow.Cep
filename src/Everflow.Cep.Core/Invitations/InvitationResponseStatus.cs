namespace Everflow.Cep.Core.Invitations;

public abstract class InvitationResponseStatus;

public class NoReply : InvitationResponseStatus;

public class Accept : InvitationResponseStatus;

public class Reject : InvitationResponseStatus;

public class Maybe : InvitationResponseStatus;

public static class InvitationResponseStatuses
{
    public static readonly NoReply NoReply = new NoReply();
    public static readonly Accept Accept = new Accept();
    public static readonly Reject Reject = new Reject();
    public static readonly Maybe Maybe = new Maybe();
    
    public static InvitationResponseStatus FromString(string value)
        => (InvitationResponseStatus)Activator
            .CreateInstance(
                typeof(InvitationResponseStatus).Assembly.FullName, 
                $"{typeof(InvitationResponseStatus).FullName!.Remove(typeof(InvitationResponseStatus).FullName.LastIndexOf('.'))}.{value}")
            .Unwrap();
}