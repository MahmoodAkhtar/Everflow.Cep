using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Events.List;

public class ListEventStatusesQueryValidator : IValidator<ListEventStatusesQuery>
{
    public Result<bool, Error> Validate(ListEventStatusesQuery value)
    {
        var errors = new List<Error>();
        if (value is null) errors.Add(ListEventStatusesQuery.Errors.IsNull);
        
        return errors.Count is 0 ? true : errors.First();
    }
}