using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Auth;

public record LoginQuery(string Username, string Password) : IRequest<Result<LoggedInDto, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("LoginQuery.IsNull", "LoginQuery is null");
    }
}