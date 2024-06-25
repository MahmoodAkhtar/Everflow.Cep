using Everflow.Cep.Core.Auth;
using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Services;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Auth;

public class LoginHandler : IRequestHandler<LoginQuery, Result<LoggedInDto, Error>>
{
    private readonly IAuthService _authService;
    private readonly IUsersRepository _usersRepository;
    private readonly IValidator<LoginQuery> _validator;

    public LoginHandler(
        IAuthService authService, 
        IUsersRepository usersRepository, 
        IValidator<LoginQuery> validator)
    {
        _authService = authService;
        _usersRepository = usersRepository;
        _validator = validator;
    }
    public async Task<Result<LoggedInDto, Error>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var authResult = await _authService.AuthenticateLoginAsync(
            new Login(request.Username, request.Password), cancellationToken);
        if (authResult.IsFailure) return authResult.Error;

        var userResult = await _usersRepository.GetByEmailAsync(request.Username, cancellationToken);
        if (userResult.IsFailure) return userResult.Error;

        var loggedInDto = new LoggedInDto(userResult.Value!.Id, request.Username);

        return loggedInDto;
    }
}