using Everflow.Cep.Application.Events;

namespace Everflow.Cep.Api.Events;

public abstract record EventStatusRecord
{
    public static EventStatusRecord FromDto(EventStatusDto status)
    {
        if (EventStatusDtos.Draft.GetType() == status.GetType()) return EventStatusRecords.Draft;
        if (EventStatusDtos.OpenToInvitation.GetType() == status.GetType()) return EventStatusRecords.OpenToInvitation;
        if (EventStatusDtos.CloseToInvitation.GetType() == status.GetType()) return EventStatusRecords.CloseToInvitation;
        if (EventStatusDtos.Finished.GetType() == status.GetType()) return EventStatusRecords.Finished;
        return EventStatusRecords.Draft;
    }

    public sealed override string ToString()
        => GetType().Name;
}

public record Draft : EventStatusRecord;

public record OpenToInvitation : EventStatusRecord;

public record CloseToInvitation : EventStatusRecord;

public record Finished : EventStatusRecord;

public static class EventStatusRecords
{
    public static readonly Draft Draft = new Draft();
    public static readonly OpenToInvitation OpenToInvitation = new OpenToInvitation();
    public static readonly CloseToInvitation CloseToInvitation = new CloseToInvitation();
    public static readonly Finished Finished = new Finished();

    public static EventStatusRecord FromString(string value)
        => (EventStatusRecord)Activator
            .CreateInstanceFrom(
                typeof(EventStatusRecord).Assembly.FullName, 
                $"{typeof(EventStatusRecord).FullName!.Remove(typeof(EventStatusRecord).FullName.LastIndexOf('.'))}.{value}")
            .Unwrap();
}