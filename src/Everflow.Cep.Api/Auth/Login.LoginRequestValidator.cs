using FluentValidation;
using FluentValidation.Results;

namespace Everflow.Cep.Api.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).Custom((value, context) =>
        {
            if (!new Core.Auth.Login.UsernameSpecification().IsSatisfiedBy(value))
                context.AddFailure(new ValidationFailure(
                    Core.Auth.Login.Errors.UsernameIsRequired.Code,
                    Core.Auth.Login.Errors.UsernameIsRequired.Description,
                    value));
        });

        RuleFor(x => x.Password).Custom((value, context) =>
        {
            if (!new Core.Auth.Login.PasswordSpecification().IsSatisfiedBy(value))
                context.AddFailure(new ValidationFailure(
                    Core.Auth.Login.Errors.PasswordIsRequired.Code,
                    Core.Auth.Login.Errors.PasswordIsRequired.Description,
                    value));
        });
    }
}