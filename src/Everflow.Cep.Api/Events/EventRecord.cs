using Everflow.Cep.Api.Users;
using Everflow.Cep.Application.Events;

namespace Everflow.Cep.Api.Events;

public record EventRecord(
    int Id,
    UserRecord CreatedByUser,
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string Status)
{
    public static EventRecord FromDto(EventDto @event)
        => new(
            @event.Id,
            UserRecord.FromDto(@event.CreatedByUser),
            @event.Name,
            @event.Description,
            @event.StartDateTime,
            @event.EndDateTime,
            EventStatusRecord.FromDto(@event.Status).ToString());
}