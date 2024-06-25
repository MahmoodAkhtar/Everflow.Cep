using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Users.Get;

public class GetUserByIdQuery(int id) : IRequest<Result<UserDto?, Error>>
{
    public int Id { get; } = id;

    public static class Errors
    {
        public static Error IsNull => Error.Conflict("GetUserByIdQuery.IsNull", "GetUserByIdQuery is null");
    }
}