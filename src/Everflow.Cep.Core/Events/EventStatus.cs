namespace Everflow.Cep.Core.Events;

public abstract class EventStatus;

public class Draft : EventStatus;

public class OpenToInvitation : EventStatus;

public class CloseToInvitation : EventStatus;

public class Finished : EventStatus;

public static class EventStatuses
{
    public static readonly Draft Draft = new Draft();
    public static readonly OpenToInvitation OpenToInvitation = new OpenToInvitation();
    public static readonly CloseToInvitation CloseToInvitation = new CloseToInvitation();
    public static readonly Finished Finished = new Finished();

    public static EventStatus FromString(string value)
        => (EventStatus)Activator
            .CreateInstance(
                typeof(EventStatus).Assembly.FullName, 
                $"{typeof(EventStatus).FullName!.Remove(typeof(EventStatus).FullName.LastIndexOf('.'))}.{value}")
            .Unwrap();
}