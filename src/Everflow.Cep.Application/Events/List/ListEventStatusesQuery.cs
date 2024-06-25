using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Events.List;

public record ListEventStatusesQuery : IRequest<Result<IEnumerable<EventStatusDto>, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("ListEventStatusesQuery.IsNull", "ListEventStatusesQuery is null");
    }
}