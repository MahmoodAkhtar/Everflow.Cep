using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Invitations;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Invitations.Create;

public class CreateInvitationCommandValidator : IValidator<CreateInvitationCommand>
{
    public Result<bool, Error> Validate(CreateInvitationCommand value)
    {
        var arg = () => value is null ? CreateInvitationCommand.Errors.IsNull : Error.None;
        
        var invitedUser = () =>
            new User.IdSpecification().IsSatisfiedBy(value.InvitedUserId)
                ? Error.None
                : User.Errors.IdMustBeGreaterThanZero;
        
        var invitedToEvent = () =>
            new Event.IdSpecification().IsSatisfiedBy(value.InvitedToEventId)
                ? Error.None
                : Event.Errors.IdMustBeGreaterThanZero;
        
        var sentDateTime = () =>
            new Invitation.SentDateTimeSpecification().IsSatisfiedBy(value.SentDateTime)
                ? Error.None
                : Invitation.Errors.SentDateTimeOutOfRange;
        
        var responseStatus = () =>
            new Invitation.ResponseStatusSpecification()
                .IsSatisfiedBy(InvitationResponseStatuses.FromString(value.ResponseStatus.GetType().Name))
                ? Error.None
                : Invitation.Errors.ResponseStatusMustBeAValidInvitationResponseStatus;
        
        var errors = new[]
            {
                arg, invitedUser, invitedToEvent, sentDateTime, responseStatus
            }
            .Select(func => func())
            .Where(error => error != Error.None)
            .ToList();
        
        return errors.Count is 0 ? true : errors.First();
    }
}