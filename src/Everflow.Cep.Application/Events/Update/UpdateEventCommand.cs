using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Events.Update;

public record UpdateEventCommand(
    int Id,
    int CreatedByUserId,
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    EventStatusDto Status) : IRequest<Result<EventDto, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("UpdateEventCommand.IsNull", "UpdateEventCommand is null");
    }
}