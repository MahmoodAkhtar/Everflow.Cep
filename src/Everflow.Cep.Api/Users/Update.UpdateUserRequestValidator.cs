using Everflow.Cep.Core.Users;
using FluentValidation;
using FluentValidation.Results;

namespace Everflow.Cep.Api.Users;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Name).Custom((value, context) =>
        {
            if (!new User.NameSpecification().IsSatisfiedBy(value)) 
                context.AddFailure(new ValidationFailure(
                    User.Errors.NameMustBeAValidFormat.Code,
                    User.Errors.NameMustBeAValidFormat.Description,
                    value));
        });
        
        RuleFor(x => x.Email).Custom((value, context) =>
        {
            if (!new User.EmailSpecification().IsSatisfiedBy(value)) 
                context.AddFailure(new ValidationFailure(
                    User.Errors.EmailMustBeAValidFormat.Code,
                    User.Errors.EmailMustBeAValidFormat.Description,
                    value));
        });    
        
        RuleFor(x => x.Password).Custom((value, context) =>
        {
            if (!new User.PasswordSpecification().IsSatisfiedBy(value)) 
                context.AddFailure(new ValidationFailure(
                    User.Errors.PasswordMustBeAValidFormat.Code,
                    User.Errors.PasswordMustBeAValidFormat.Description,
                    value));
        });
    }
}