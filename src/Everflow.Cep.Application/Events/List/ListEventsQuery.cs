using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Events.List;

public record ListEventsQuery(int Offset, int Limit) : IRequest<Result<IEnumerable<EventDto>, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("ListEventsQuery.IsNull", "ListEventsQuery is null");
    }
}