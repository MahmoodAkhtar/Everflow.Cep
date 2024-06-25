using Everflow.Cep.Core.Auth;
using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Services;

public interface IAuthService
{
    Task<Result<bool, Error>> AuthenticateLoginAsync(Login login, CancellationToken cancellationToken);
}