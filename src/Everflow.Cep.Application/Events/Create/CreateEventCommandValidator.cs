using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Events.Create;

public class CreateEventCommandValidator : IValidator<CreateEventCommand>
{
    public Result<bool, Error> Validate(CreateEventCommand value)
    {
        var arg = () => value is null ? CreateEventCommand.Errors.IsNull : Error.None;

        var createdByUser = () =>
            new User.IdSpecification().IsSatisfiedBy(value.CreatedByUserId)
                ? Error.None
                : User.Errors.IdMustBeGreaterThanZero;

        var name = () =>
            new Event.NameSpecification().IsSatisfiedBy(value.Name) ? Error.None : Event.Errors.NameMustBeAValidFormat;
        
        var description = () =>
            new Event.DescriptionSpecification().IsSatisfiedBy(value.Description)
                ? Error.None
                : Event.Errors.DescriptionMustBeAValidFormat;
        
        var startDateTime = () =>
            new Event.StartDateTimeSpecification().IsSatisfiedBy(value.StartDateTime)
                ? Error.None
                : Event.Errors.StartDateTimeOutOfRange;
        
        var endDateTime = () =>
            new Event.EndDateTimeSpecification().IsSatisfiedBy(value.EndDateTime)
                ? Error.None
                : Event.Errors.EndDateTimeOutOfRange;
        
        var startDateTimeBeforeEndDateTime = () =>
            new Event.StartDateTimeMustBeBeforeEndDateTimeSpecification()
                .IsSatisfiedBy(new Tuple<DateTime, DateTime>(value.StartDateTime, value.EndDateTime))
                ? Error.None
                : Event.Errors.StartDateTimeMustBeBeforeEndDateTime;

        var status = () =>
            new Event.StatusSpecification().IsSatisfiedBy(EventStatuses.FromString(value.Status.GetType().Name))
                ? Error.None
                : Event.Errors.StatusMustBeAValidEventStatus;
        
        var errors = new[]
            {
                arg, createdByUser, name, description, startDateTime, endDateTime, startDateTimeBeforeEndDateTime,
                status
            }
            .Select(func => func())
            .Where(error => error != Error.None)
            .ToList();

        return errors.Count is 0 ? true : errors.First();
    }
}