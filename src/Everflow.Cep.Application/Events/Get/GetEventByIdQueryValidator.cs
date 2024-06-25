using Everflow.Cep.Core.Events;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Events.Get;

public class GetEventByIdQueryValidator : IValidator<GetEventByIdQuery>
{
    public Result<bool, Error> Validate(GetEventByIdQuery value)
    {
        var arg = () => value is null 
            ? GetEventByIdQuery.Errors.IsNull 
            : Error.None;
        
        var id = () =>
            new Event.IdSpecification().IsSatisfiedBy(value.Id) 
                ? Error.None 
                : Event.Errors.IdMustBeGreaterThanZero;

        var errors = new[] { arg, id }
            .Select(func => func())
            .Where(error => error != Error.None)
            .ToList();

        return errors.Count is 0 ? true : errors.First();
    }
}