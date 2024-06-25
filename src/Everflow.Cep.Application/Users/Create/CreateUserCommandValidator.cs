using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Users.Create;

public class CreateUserCommandValidator : IValidator<CreateUserCommand>
{
    public Result<bool, Error> Validate(CreateUserCommand value)
    {
        var errors = new List<Error>();
        
        if (value is null)
            errors.Add(CreateUserCommand.Errors.IsNull);
        
        if (!new User.NameSpecification().IsSatisfiedBy(value!.Name))
            errors.Add(User.Errors.NameMustBeAValidFormat);
        
        if (!new User.EmailSpecification().IsSatisfiedBy(value!.Email))
            errors.Add(User.Errors.EmailMustBeAValidFormat);
        
        if (!new User.PasswordSpecification().IsSatisfiedBy(value!.Password))
            errors.Add(User.Errors.PasswordMustBeAValidFormat);

        return errors.Count is 0 ? true : errors.First();
    }
}