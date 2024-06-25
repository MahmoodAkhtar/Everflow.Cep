using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Events.Get;

public record GetEventByIdQuery(int Id) : IRequest<Result<EventDto?, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("GetEventByIdQuery.IsNull", "GetEventByIdQuery is null");
    }
}