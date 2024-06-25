using Everflow.Cep.Core.Services;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Invitations.List;

public class ListInvitationsQueryValidator(OffsetPaginationSettings offsetPaginationSettings) : IValidator<ListInvitationsQuery>
{
    public Result<bool, Error> Validate(ListInvitationsQuery value)
    {
        var errors = new List<Error>(); 
        if (value is null) errors.Add(ListInvitationsQuery.Errors.IsNull);
        
        if (!new OffsetPagination.OffsetSpecification().IsSatisfiedBy(value!.Offset))
            errors.Add(OffsetPagination.Errors.OffsetMustBeZeroOrMore);
        
        if (!new OffsetPagination.LimitSpecification(offsetPaginationSettings).IsSatisfiedBy(value!.Limit))
            errors.Add(OffsetPagination.Errors.LimitIsOutOfRange);

        return errors.Count is 0 ? true : errors.First();
    }
}