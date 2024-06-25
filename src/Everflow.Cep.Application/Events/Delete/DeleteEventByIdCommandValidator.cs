using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Events.Delete;

public class DeleteEventByIdCommandValidator: IValidator<DeleteEventByIdCommand>
{
    public Result<bool, Error> Validate(DeleteEventByIdCommand value)
    {
        var arg = () => value is null ? DeleteEventByIdCommand.Errors.IsNull : Error.None;
        
        var id = () => new User.IdSpecification().IsSatisfiedBy(value.Id) 
            ? Error.None 
            : Event.Errors.IdMustBeGreaterThanZero;

        var errors = new[] { arg, id }
            .Select(func => func())
            .Where(error => error != Error.None)
            .ToList();

        return errors.Count is 0 ? true : errors.First();
    }
}