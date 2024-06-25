using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Events.Create;

public record CreateEventCommand(
    int CreatedByUserId,
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    EventStatusDto Status) : IRequest<Result<int, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict(
            "CreateEventCommand.IsNull", 
            "CreateEventCommand is null");
    }
}