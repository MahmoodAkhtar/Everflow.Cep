using System.Linq.Expressions;
using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Invitations;

public class Invitation
{
    public int Id { get; private set; }
    public User InvitedUser { get; private set; } = null!;
    public Event InvitedToEvent { get; private set; }
    public DateTime SentDateTime { get; private set; }
    public InvitationResponseStatus ResponseStatus { get; private set; } = null!;

    public Invitation() // for Ef
    {
    }

    public Invitation(
        int id,
        User invitedUser,
        Event invitedToEvent,
        DateTime sentDateTime,
        InvitationResponseStatus responseStatus)
        : this(
            invitedUser,
            invitedToEvent,
            sentDateTime,
            responseStatus)
    {
        Id = Assign.OrThrowArgumentException(
            id,
            new IdSpecification(),
            Errors.IdMustBeGreaterThanZero);
    }

    public Invitation(
        User invitedUser,
        Event invitedToEvent,
        DateTime sentDateTime,
        InvitationResponseStatus? responseStatus = null)
    {
        InvitedUser = Assign.OrThrowArgumentException(
            invitedUser,
            new InvitedUserSpecification(),
            Errors.InvitedUserIsNull);

        _ = Assign.OrThrowArgumentException(
            invitedToEvent,
            new InvitedToEventSpecification(),
            Errors.InvitedToEventIsNull);

        InvitedToEvent = Assign.OrThrowArgumentException(
            invitedToEvent,
            new InvitedToEventStatusMustBeOpenForInvitationSpecification(),
            Errors.InvitedToEventStatusMustBeOpenForInvitation);

        _ = Assign.OrThrowArgumentException(
            new Tuple<User, User>(invitedUser, invitedToEvent.CreatedByUser),
            new InvitedUserCannotBeInvitedToEventCreatedBySameUserSpecification(),
            Errors.InvitedUserCannotBeInvitedToEventCreatedBySameUser);

        SentDateTime = Assign.OrThrowArgumentException(
            sentDateTime,
            new SentDateTimeSpecification(),
            Errors.InvitedToEventIsNull);

        ResponseStatus = Assign.OrThrowArgumentException(
            responseStatus ?? InvitationResponseStatuses.NoReply,
            new ResponseStatusSpecification(),
            Errors.ResponseStatusMustBeAValidInvitationResponseStatus);
    }

    private void SetInvitedUser(User invitedUser)
    {
        InvitedUser = Assign.OrThrowArgumentException(
            invitedUser,
            new InvitedUserSpecification(),
            Errors.InvitedUserIsNull);
    }

    private void SetInvitedToEvent(Event invitedToEvent)
    {
        _ = Assign.OrThrowArgumentException(
            invitedToEvent,
            new InvitedToEventSpecification(),
            Errors.InvitedToEventIsNull);

        InvitedToEvent = Assign.OrThrowArgumentException(
            invitedToEvent,
            new InvitedToEventStatusMustBeOpenForInvitationSpecification(),
            Errors.InvitedToEventStatusMustBeOpenForInvitation);
    }

    public void SetInvitedUserAndInvitedToEvent(User invitedUser, Event invitedToEvent)
    {
        SetInvitedUser(invitedUser);
        SetInvitedToEvent(invitedToEvent);

        _ = Assign.OrThrowArgumentException(
            new Tuple<User, User>(invitedUser, invitedToEvent.CreatedByUser),
            new InvitedUserCannotBeInvitedToEventCreatedBySameUserSpecification(),
            Errors.InvitedUserCannotBeInvitedToEventCreatedBySameUser);
    }

    public void SetSentDateTime(DateTime sentDateTime)
    {
        SentDateTime = Assign.OrThrowArgumentException(
            sentDateTime,
            new SentDateTimeSpecification(),
            Errors.InvitedToEventIsNull);
    }

    public void SetResponseStatus(InvitationResponseStatus responseStatus)
    {
        ResponseStatus = Assign.OrThrowArgumentException(
            responseStatus ?? InvitationResponseStatuses.NoReply,
            new ResponseStatusSpecification(),
            Errors.ResponseStatusMustBeAValidInvitationResponseStatus);    
    }
    
    public class IdSpecification : Specification<int>
    {
        public override Expression<Func<int, bool>> ToExpression()
            => x => x > 0;
    }

    public class InvitedUserSpecification : Specification<User>
    {
        public override Expression<Func<User, bool>> ToExpression()
            => x => x != null;
    }

    public class InvitedToEventSpecification : Specification<Event>
    {
        public override Expression<Func<Event, bool>> ToExpression()
            => x => x != null;
    }

    public class InvitedToEventStatusMustBeOpenForInvitationSpecification : Specification<Event>
    {
        public override Expression<Func<Event, bool>> ToExpression()
            => x => x.Status != EventStatuses.OpenToInvitation;
    }

    public class SentDateTimeSpecification : Specification<DateTime>
    {
        public override Expression<Func<DateTime, bool>> ToExpression()
            => x => x > DateTime.MinValue && x < DateTime.MaxValue;
    }

    public class ResponseStatusSpecification : Specification<InvitationResponseStatus>
    {
        public override Expression<Func<InvitationResponseStatus, bool>> ToExpression()
            => x => x != null
                    && (x.GetType() == InvitationResponseStatuses.Accept.GetType()
                        || x.GetType() == InvitationResponseStatuses.NoReply.GetType()
                        || x.GetType() == InvitationResponseStatuses.Maybe.GetType()
                        || x.GetType() == InvitationResponseStatuses.Reject.GetType());
    }

    public class InvitedUserCannotBeInvitedToEventCreatedBySameUserSpecification : Specification<Tuple<User, User>>
    {
        public override Expression<Func<Tuple<User, User>, bool>> ToExpression()
            => x => x.Item1.Id != x.Item2.Id;
    }

    public static class Errors
    {
        public static Error NotFound(int id) => Error.NotFound(
            "Invitation.NotFound", $"The Invitation with the Id = {id} was not found");

        public static Error IdMustBeGreaterThanZero => Error.Validation(
            "Event.IdMustBeGreaterThanZero", "Event Id must be greater than zero");

        public static Error InvitedUserIsNull => Error.Validation(
            "Invitation.InvitedUserIsNull", "Invitation InvitedUserIsNull is null");

        public static Error InvitedToEventIsNull => Error.Validation(
            "Invitation.InvitedToEventIsNull", "Invitation InvitedToEventIsNull is null");

        public static Error SentDateTimeOutOfRange => Error.Validation(
            "Invitation.SentDateTimeOutOfRange", "Invitation SentDateTime is out of range");

        public static Error ResponseStatusMustBeAValidInvitationResponseStatus => Error.Validation(
            "Invitation.ResponseStatusMustBeAValidInvitationResponseStatus",
            "Invitation ResponseStatus must be a valid InvitationResponseStatus");

        public static Error InvitedUserCannotBeInvitedToEventCreatedBySameUser => Error.Validation(
            "Invitation.InvitedUserCannotBeInvitedToEventCreatedBySameUser",
            "Invitation InvitedUser cannot be invited to Event created by the same User");

        public static Error InvitedToEventStatusMustBeOpenForInvitation => Error.Validation(
            "Invitation.InvitedToEventStatusMustBeOpenForInvitation",
            "Invitation InvitedToEvent Status must be OpenForInvitation");

        public static Error IsNull => Error.Validation(
            "Invitation.IsNull", "Invitation is null");
    }
}