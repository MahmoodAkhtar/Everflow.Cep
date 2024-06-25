using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Services;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Users.Create;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<int, Error>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IValidator<CreateUserCommand> _validator;
    private readonly IUserRulesService _userRules;

    public CreateUserHandler(IUsersRepository usersRepository, IValidator<CreateUserCommand> validator, IUserRulesService userRules)
    {
        _usersRepository = usersRepository;
        _validator = validator;
        _userRules = userRules;
    }

    public async Task<Result<int, Error>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var ruleResult = await _userRules.CheckIsEmailUniqueAsync(request.Email, cancellationToken);
        if (ruleResult.IsFailure) return ruleResult.Error;

        var addResult = await _usersRepository.AddAsync(new User(request.Name, request.Email, request.Password), cancellationToken);

        return addResult;
    }

    private async Task<Result<bool, Error>> RuleEmailMustBeUniqueAsync(string email,
        CancellationToken cancellationToken)
    {
        var isEmailUniqueResult = await _usersRepository.IsEmailUniqueAsync(email, cancellationToken);

        return isEmailUniqueResult.Match<Result<bool, Error>>(
            isEmailUnique => !isEmailUnique ? User.Errors.EmailMustBeUnique : true,
            error => error);
    }
}

