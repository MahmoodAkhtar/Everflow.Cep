using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Users.Delete;

public class DeleteUserByIdValidator : IValidator<DeleteUserByIdCommand>
{
    public Result<bool, Error> Validate(DeleteUserByIdCommand value)
    {
        var errors = new List<Error>();

        if (value is null)
            errors.Add(DeleteUserByIdCommand.Errors.IsNull);
        
        if (!new User.IdSpecification().IsSatisfiedBy(value!.Id))
            errors.Add(User.Errors.IdMustBeGreaterThanZero);
        
        return errors.Count is 0 ? true : errors.First();
    }
}