using Everflow.Cep.Application.Users;
using Everflow.Cep.Core.Events;

namespace Everflow.Cep.Application.Events;

public record EventDto(
    int Id,
    UserDto CreatedByUser,
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    EventStatusDto Status)
{
    public static EventDto FromEvent(Event @event)
        => new (
            @event.Id,
            UserDto.FromUser(@event.CreatedByUser), 
            @event.Name, 
            @event.Description, 
            @event.StartDateTime, 
            @event.EndDateTime, 
            EventStatusDto.FromEventStatus(@event.Status));
}

