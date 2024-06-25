using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Users.Update;

public class UpdateUserCommandValidator : IValidator<UpdateUserCommand>
{
    public Result<bool, Error> Validate(UpdateUserCommand value)
    {
        var errors = new List<Error>();
        
        if (value is null)
            errors.Add(UpdateUserCommand.Errors.IsNull);

        if (!new User.IdSpecification().IsSatisfiedBy(value!.Id))
            errors.Add(User.Errors.IdMustBeGreaterThanZero);
        
        if (!new User.NameSpecification().IsSatisfiedBy(value!.Name))
            errors.Add(User.Errors.NameMustBeAValidFormat);
        
        if (!new User.EmailSpecification().IsSatisfiedBy(value!.Email))
            errors.Add(User.Errors.EmailMustBeAValidFormat);
        
        if (!new User.PasswordSpecification().IsSatisfiedBy(value!.Password))
            errors.Add(User.Errors.PasswordMustBeAValidFormat);

        return errors.Count is 0 ? true : errors.First();
    }
}