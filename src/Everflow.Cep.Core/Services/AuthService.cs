using Everflow.Cep.Core.Auth;
using Everflow.Cep.Core.Interfaces;
using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUsersRepository _usersRepository;

    public AuthService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<Result<bool, Error>> AuthenticateLoginAsync(Login login, CancellationToken cancellationToken)
    {
        var userResult = await _usersRepository.GetByEmailAsync(login.Username, cancellationToken);
        if (userResult.IsFailure) return userResult.Error;

        var isValid = userResult.Value is not null && userResult.Value!.Password == login.Password;

        return isValid ? true : Login.Errors.LoginFailed;
    }
}