using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Services;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Users.Update;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserDto, Error>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IValidator<UpdateUserCommand> _validator;
    private readonly IUserRulesService _userRules;

    public UpdateUserHandler(IUsersRepository usersRepository, IValidator<UpdateUserCommand> validator, IUserRulesService userRules)
    {
        _usersRepository = usersRepository;
        _validator = validator;
        _userRules = userRules;
    }
    
    public async Task<Result<UserDto, Error>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var checkResult = await _userRules.CheckIsEmailUniqueForIdAsync(request.Id, request.Email, cancellationToken);
        if (checkResult.IsFailure) return checkResult.Error;
        
        var updateResult = await _usersRepository
            .UpdateAsync(new User(request.Id, request.Name, request.Email, request.Password), cancellationToken);

        return updateResult.Match<Result<UserDto, Error>>(
            user => UserDto.FromUser(user),
            error => error);
    }
}