using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Users;
using FluentValidation;
using FluentValidation.Results;

namespace Everflow.Cep.Api.Events;


public class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequest>
{
    public UpdateEventRequestValidator()
    {
        RuleFor(x => x.CreatedByUserId).Custom((value, context) =>
        {
            if (!new User.IdSpecification().IsSatisfiedBy(value))
                context.AddFailure(new ValidationFailure(
                    User.Errors.IdMustBeGreaterThanZero.Code,
                    User.Errors.IdMustBeGreaterThanZero.Description,
                    value));
        });

        RuleFor(x => x.Name).Custom((value, context) =>
        {
            if (!new Event.NameSpecification().IsSatisfiedBy(value))
                context.AddFailure(new ValidationFailure(
                    Event.Errors.NameMustBeAValidFormat.Code,
                    Event.Errors.NameMustBeAValidFormat.Description,
                    value));
        });

        RuleFor(x => x.Description).Custom((value, context) =>
        {
            if (!new Event.DescriptionSpecification().IsSatisfiedBy(value))
                context.AddFailure(new ValidationFailure(
                    Event.Errors.DescriptionMustBeAValidFormat.Code,
                    Event.Errors.DescriptionMustBeAValidFormat.Description,
                    value));
        });

        RuleFor(x => x.StartDateTime).Custom((value, context) =>
        {
            if (!new Event.StartDateTimeSpecification().IsSatisfiedBy(value))
                context.AddFailure(new ValidationFailure(
                    Event.Errors.StartDateTimeOutOfRange.Code,
                    Event.Errors.StartDateTimeOutOfRange.Description,
                    value));
        });

        RuleFor(x => x.EndDateTime).Custom((value, context) =>
        {
            if (!new Event.EndDateTimeSpecification().IsSatisfiedBy(value))
                context.AddFailure(new ValidationFailure(
                    Event.Errors.EndDateTimeOutOfRange.Code,
                    Event.Errors.EndDateTimeOutOfRange.Description,
                    value));
        });

        RuleFor(x => x).Custom((value, context) =>
        {
            if (!new Event.StartDateTimeMustBeBeforeEndDateTimeSpecification().IsSatisfiedBy(
                    new Tuple<DateTime, DateTime>(value.StartDateTime, value.EndDateTime)))
                context.AddFailure(new ValidationFailure(
                    Event.Errors.StartDateTimeMustBeBeforeEndDateTime.Code,
                    Event.Errors.StartDateTimeMustBeBeforeEndDateTime.Description,
                    value));
        });

        RuleFor(x => x.Status).Custom((value, context) =>
        {
            if (!new Event.StatusSpecification().IsSatisfiedBy(EventStatuses.FromString(value)))
                context.AddFailure(new ValidationFailure(
                    Event.Errors.StatusMustBeAValidEventStatus.Code,
                    Event.Errors.StatusMustBeAValidEventStatus.Description,
                    value));
        });
    }
}