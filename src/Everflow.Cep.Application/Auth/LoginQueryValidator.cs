using Everflow.Cep.Core.Auth;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Auth;

public class LoginQueryValidator : IValidator<LoginQuery>
{
    public Result<bool, Error> Validate(LoginQuery value)
    {
        var errors = new List<Error>();
        if (value is null) errors.Add(LoginQuery.Errors.IsNull);
        
        if (!new Login.UsernameSpecification().IsSatisfiedBy(value!.Username))
            errors.Add(Login.Errors.UsernameIsRequired);
        
        if (!new Login.PasswordSpecification().IsSatisfiedBy(value!.Password))
            errors.Add(Login.Errors.PasswordIsRequired);
        
        return errors.Count is 0 ? true : errors.First();
    }
}