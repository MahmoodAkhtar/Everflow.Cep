using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Users.Get;

public class GetUserByIdQueryValidator : IValidator<GetUserByIdQuery>
{
    public Result<bool, Error> Validate(GetUserByIdQuery value)
    {
        var errors = new List<Error>();
        
        if (value is null)
            errors.Add(GetUserByIdQuery.Errors.IsNull);
        
        if (!new User.IdSpecification().IsSatisfiedBy(value!.Id))
            errors.Add(User.Errors.IdMustBeGreaterThanZero);

        return errors.Count is 0 ? true : errors.First();
    }
}