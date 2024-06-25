using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Users.List;

public record ListUsersQuery(int Offset, int Limit) : IRequest<Result<IEnumerable<UserDto>, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("ListUsersQuery.IsNull", "ListUsersQuery is null");
    }
}