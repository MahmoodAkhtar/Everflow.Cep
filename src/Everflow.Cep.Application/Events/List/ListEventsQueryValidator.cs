using Everflow.Cep.Core.Services;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Events.List;

public class ListEventsQueryValidator(OffsetPaginationSettings offsetPaginationSettings) : IValidator<ListEventsQuery>
{
    public Result<bool, Error> Validate(ListEventsQuery value)
    {
        var errors = new List<Error>(); 
        if (value is null) errors.Add(ListEventsQuery.Errors.IsNull);
        
        if (!new OffsetPagination.OffsetSpecification().IsSatisfiedBy(value!.Offset))
            errors.Add(OffsetPagination.Errors.OffsetMustBeZeroOrMore);
        
        if (!new OffsetPagination.LimitSpecification(offsetPaginationSettings).IsSatisfiedBy(value!.Limit))
            errors.Add(OffsetPagination.Errors.LimitIsOutOfRange);

        return errors.Count is 0 ? true : errors.First();
    }
}