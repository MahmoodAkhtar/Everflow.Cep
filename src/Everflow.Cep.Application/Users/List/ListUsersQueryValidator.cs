using Everflow.Cep.Core.Services;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Users.List;

public class ListUsersQueryValidator(OffsetPaginationSettings offsetPaginationSettings) : IValidator<ListUsersQuery>
{
    public Result<bool, Error> Validate(ListUsersQuery value)
    {
        var errors = new List<Error>(); 
        if (value is null) errors.Add(ListUsersQuery.Errors.IsNull);
        
        if (!new OffsetPagination.OffsetSpecification().IsSatisfiedBy(value!.Offset))
            errors.Add(OffsetPagination.Errors.OffsetMustBeZeroOrMore);
        
        if (!new OffsetPagination.LimitSpecification(offsetPaginationSettings).IsSatisfiedBy(value!.Limit))
            errors.Add(OffsetPagination.Errors.LimitIsOutOfRange);

        return errors.Count is 0 ? true : errors.First();
    }
}