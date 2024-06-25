using System.Linq.Expressions;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Events;

public class Event
{
    public int Id { get; private set; }
    public User CreatedByUser { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }
    public EventStatus Status { get; private set; } = null!;

    public Event() // for Ef
    {
    }
    
    public Event(
        int id, 
        User createdByUser, 
        string name, 
        string description,
        DateTime startDateTime, 
        DateTime endDateTime, 
        EventStatus status) 
        : this(
            createdByUser, 
            name, 
            description, 
            startDateTime, 
            endDateTime, 
            status)
    {
        Id = Assign.OrThrowArgumentException(
            id, 
            new IdSpecification(), 
            Errors.IdMustBeGreaterThanZero);
    }
    
    public Event(
        User createdByUser, 
        string name, 
        string description, 
        DateTime startDateTime, 
        DateTime endDateTime, 
        EventStatus? status = null)
    {
        CreatedByUser = Assign.OrThrowArgumentException(
            createdByUser, 
            new CreatedByUserSpecification(), 
            Errors.CreatedByUserIsNull);
        
        Name = Assign.OrThrowArgumentException(
            name, 
            new User.NameSpecification(), 
            Errors.NameMustBeAValidFormat);
        
        Description = Assign.OrThrowArgumentException(
            description, 
            new DescriptionSpecification(), 
            Errors.DescriptionMustBeAValidFormat);
        
        StartDateTime = Assign.OrThrowArgumentException(
            startDateTime, 
            new StartDateTimeSpecification(), 
            Errors.StartDateTimeOutOfRange);
        
        EndDateTime = Assign.OrThrowArgumentException(
            endDateTime, 
            new EndDateTimeSpecification(), 
            Errors.EndDateTimeOutOfRange);
        
        _ = Assign.OrThrowArgumentException(
            new Tuple<DateTime, DateTime>(startDateTime, endDateTime), 
            new StartDateTimeMustBeBeforeEndDateTimeSpecification(), 
            Errors.StartDateTimeMustBeBeforeEndDateTime);
        
        Status = Assign.OrThrowArgumentException(
            status ?? EventStatuses.Draft, 
            new StatusSpecification(), 
            Errors.StatusMustBeAValidEventStatus);
    }
    
    public void SetCreatedByUser(User createdByUser)
    {
        CreatedByUser = Assign.OrThrowArgumentException(
            createdByUser, 
            new CreatedByUserSpecification(), 
            Errors.CreatedByUserIsNull);
    } 
    
    public void SetName(string name)
    {
        Name = Assign.OrThrowArgumentException(
            name, 
            new NameSpecification(), 
            Errors.NameMustBeAValidFormat);
    }   
    
    public void SetDescription(string description)
    {
        Name = Assign.OrThrowArgumentException(
            description, 
            new DescriptionSpecification(), 
            Errors.DescriptionMustBeAValidFormat);
    }
    
    private void SetStartDateTime(DateTime startDate)
    {
        StartDateTime = Assign.OrThrowArgumentException(
            startDate, 
            new StartDateTimeSpecification(), 
            Errors.StartDateTimeOutOfRange);
    } 
    
    private void SetEndDateTime(DateTime endDate)
    {
        EndDateTime = Assign.OrThrowArgumentException(
            endDate, 
            new EndDateTimeSpecification(), 
            Errors.EndDateTimeOutOfRange);
    }

    public void SetStartDateTimeAndEndDateTime(DateTime startDateTime, DateTime endDateTime)
    {
        SetStartDateTime(startDateTime);
        SetEndDateTime(endDateTime);
        
        _ = Assign.OrThrowArgumentException(
            new Tuple<DateTime, DateTime>(startDateTime, endDateTime), 
            new StartDateTimeMustBeBeforeEndDateTimeSpecification(), 
            Errors.StartDateTimeMustBeBeforeEndDateTime);
    }
    
    public void SetStatus(EventStatus status)
    {
        Status = Assign.OrThrowArgumentException(
            status, 
            new StatusSpecification(), 
            Errors.StatusMustBeAValidEventStatus);
    }
    
    public class IdSpecification : Specification<int>
    {
        public override Expression<Func<int, bool>> ToExpression()
            => x => x > 0;
    }
    
    public class CreatedByUserSpecification : Specification<User>
    {
        public override Expression<Func<User, bool>> ToExpression()
            => x => x != null;
    }
    
    public class NameSpecification : Specification<string>
    {
        public override Expression<Func<string, bool>> ToExpression()
            => x => !string.IsNullOrWhiteSpace(x)
                    && x.Length <= 100;
    }
    
    public class DescriptionSpecification : Specification<string>
    {
        public override Expression<Func<string, bool>> ToExpression()
            => x => !string.IsNullOrWhiteSpace(x);
    }
    
    public class StartDateTimeSpecification : Specification<DateTime>
    {
        public override Expression<Func<DateTime, bool>> ToExpression()
            => x => x > DateTime.MinValue && x < DateTime.MaxValue;
    }
    
    public class EndDateTimeSpecification : Specification<DateTime>
    {
        public override Expression<Func<DateTime, bool>> ToExpression()
            => x => x > DateTime.MinValue && x < DateTime.MaxValue;
    }
    
    public class StartDateTimeMustBeBeforeEndDateTimeSpecification : Specification<Tuple<DateTime, DateTime>>
    {
        public override Expression<Func<Tuple<DateTime, DateTime>, bool>> ToExpression()
            => x => x.Item1 < x.Item2;
    }
    
    public class StatusSpecification : Specification<EventStatus>
    {
        public override Expression<Func<EventStatus, bool>> ToExpression()
            => x => x != null
                    && (x.GetType() == EventStatuses.Draft.GetType()
                        || x.GetType() == EventStatuses.OpenToInvitation.GetType()
                        || x.GetType() == EventStatuses.CloseToInvitation.GetType()
                        || x.GetType() == EventStatuses.Finished.GetType());
    }
    
    public static class Errors
    {
        public static Error NotFound(int id) => Error.NotFound(
            "Event.NotFound", $"The Event with the Id = {id} was not found");
            
        public static Error IdMustBeGreaterThanZero => Error.Validation(
            "Event.IdMustBeGreaterThanZero", "Event Id must be greater than zero");  
            
        public static Error CreatedByUserIsNull => Error.Validation(
            "Event.CreatedByUserIsNull", "Event CreatedByUser is null"); 
        
        public static Error NameMustBeAValidFormat => Error.Validation(
            "Event.NameMustBeAValidFormat", "Event Name must be a valid format");
        
        public static Error DescriptionMustBeAValidFormat => Error.Validation(
            "Event.DescriptionMustBeAValidFormat", "Event Description must be a valid format");
        
        public static Error StartDateTimeOutOfRange => Error.Validation(
            "Event.StartDateTimeOutOfRange", "Event StartDateTime is out of range");

        public static Error EndDateTimeOutOfRange => Error.Validation(
            "Event.EndDateTimeOutOfRange", "Event EndDateTime is out of range");

        public static Error StartDateTimeMustBeBeforeEndDateTime => Error.Validation(
            "Event.StartDateTimeMustBeBeforeEndDateTime", "Event StartDateTime must be before the EndDateTime");

        public static Error StatusMustBeAValidEventStatus => Error.Validation(
            "Event.StatusMustBeAValidEventStatus", "Event Status must be a valid EventStatus");
        
        public static Error IsNull => Error.Validation(
            "Event.IsNull", "Event is null");
    }
}