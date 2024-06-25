using Everflow.Cep.Core.Services;
using FluentValidation;
using FluentValidation.Results;

namespace Everflow.Cep.Api.Invitations;

public class ListInvitationsRequestValidator: AbstractValidator<ListInvitationsRequest>
{
    public ListInvitationsRequestValidator(OffsetPaginationSettings offsetPaginationSettings)
    {
        RuleFor(x => x.Offset).Custom((value, context) =>
        {
            if (!new OffsetPagination.OffsetSpecification().IsSatisfiedBy(value))
                context.AddFailure(new ValidationFailure(
                    OffsetPagination.Errors.OffsetMustBeZeroOrMore.Code,
                    OffsetPagination.Errors.OffsetMustBeZeroOrMore.Description,
                    value));
        });

        RuleFor(x => x.Limit).Custom((value, context) =>
        {
            if (!new OffsetPagination.LimitSpecification(offsetPaginationSettings).IsSatisfiedBy(value))
                context.AddFailure(new ValidationFailure(
                    OffsetPagination.Errors.LimitIsOutOfRange.Code,
                    OffsetPagination.Errors.LimitIsOutOfRange.Description,
                    value));
        });
    }
}