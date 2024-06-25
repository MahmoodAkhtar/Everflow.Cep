using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Services;

public class UserRulesService : IUserRulesService
{
    private readonly IUsersRepository _usersRepository;

    public UserRulesService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    
    public async Task<Result<bool, Error>> CheckIsEmailUniqueAsync(string email, CancellationToken cancellationToken)
    {
        if (!new User.EmailSpecification().IsSatisfiedBy(email))
            return User.Errors.EmailMustBeAValidFormat;

        var result = await _usersRepository.IsEmailUniqueAsync(email, cancellationToken);

        return result.Match<Result<bool, Error>>(
            check => !check ? User.Errors.EmailMustBeUnique : true,
            error => error);
    }

    public async Task<Result<bool, Error>> CheckIsEmailUniqueForIdAsync(int id, string email, CancellationToken cancellationToken)
    {
        if (!new User.IdSpecification().IsSatisfiedBy(id))
            return User.Errors.IdMustBeGreaterThanZero;
        
        if (!new User.EmailSpecification().IsSatisfiedBy(email))
            return User.Errors.EmailMustBeAValidFormat;

        var getResult = await _usersRepository.GetByIdAsync(id, cancellationToken);
        if (getResult.IsFailure) return getResult.Error;
        if (getResult.Value?.Email == email) return true;
        
        var isEmailUniqueResult = await _usersRepository.IsEmailUniqueAsync(email, cancellationToken);
        
        return isEmailUniqueResult.Match<Result<bool, Error>>(
            check => !check ? User.Errors.EmailMustBeUnique : true,
            error => error);
    }

    public Task<Result<bool, Error>> CheckShouldUserBeDeletedAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}