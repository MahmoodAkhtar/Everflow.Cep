using Everflow.Cep.Core.Events;

namespace Everflow.Cep.Application.Events;

public abstract record EventStatusDto
{
    public static EventStatusDto FromEventStatus(EventStatus status)
    {
        if (EventStatuses.Draft.GetType() == status.GetType()) return EventStatusDtos.Draft;
        if (EventStatuses.OpenToInvitation.GetType() == status.GetType()) return EventStatusDtos.OpenToInvitation;
        if (EventStatuses.CloseToInvitation.GetType() == status.GetType()) return EventStatusDtos.CloseToInvitation;
        if (EventStatuses.Finished.GetType() == status.GetType()) return EventStatusDtos.Finished;
        return EventStatusDtos.Draft;
    }
}

public record Draft : EventStatusDto;

public record OpenToInvitation : EventStatusDto;

public record CloseToInvitation : EventStatusDto;

public record Finished : EventStatusDto;

public static class EventStatusDtos
{
    public static readonly Draft Draft = new Draft();
    public static readonly OpenToInvitation OpenToInvitation = new OpenToInvitation();
    public static readonly CloseToInvitation CloseToInvitation = new CloseToInvitation();
    public static readonly Finished Finished = new Finished();

    public static EventStatusDto FromString(string value)
        => (EventStatusDto)Activator.CreateInstance(
                typeof(EventStatusDto).Assembly.FullName, 
                $"{typeof(EventStatusDto).FullName!
                    .Remove(typeof(EventStatusDto).FullName!.LastIndexOf('.'))}.{value}")
            .Unwrap();
}