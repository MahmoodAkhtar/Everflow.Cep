using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Invitations;
using Everflow.Cep.Core.Users;
using FluentValidation;
using FluentValidation.Results;

namespace Everflow.Cep.Api.Invitations;

public class CreateInvitationRequestValidator : AbstractValidator<CreateInvitationRequest>
{
    public CreateInvitationRequestValidator()
    {
        RuleFor(x => x.InvitedUserId).Custom((value, context) =>
        {
            if (!new Invitation.IdSpecification().IsSatisfiedBy(value))
                context.AddFailure(new ValidationFailure(
                    User.Errors.IdMustBeGreaterThanZero.Code,
                    User.Errors.IdMustBeGreaterThanZero.Description,
                    value));
        });
        
        RuleFor(x => x.InvitedToEventId).Custom((value, context) =>
        {
            if (!new Event.IdSpecification().IsSatisfiedBy(value))
                context.AddFailure(new ValidationFailure(
                    Event.Errors.IdMustBeGreaterThanZero.Code,
                    Event.Errors.IdMustBeGreaterThanZero.Description,
                    value));
        });
        
        RuleFor(x => x.SentDateTime).Custom((value, context) =>
        {
            if (!new Invitation.SentDateTimeSpecification().IsSatisfiedBy(value))
                context.AddFailure(new ValidationFailure(
                    Invitation.Errors.SentDateTimeOutOfRange.Code,
                    Invitation.Errors.SentDateTimeOutOfRange.Description,
                    value));
        });
        
        RuleFor(x => x.ResponseStatus).Custom((value, context) =>
        {
            if (!new Invitation.ResponseStatusSpecification()
                    .IsSatisfiedBy(InvitationResponseStatuses.FromString(value)))
                context.AddFailure(new ValidationFailure(
                    Invitation.Errors.SentDateTimeOutOfRange.Code,
                    Invitation.Errors.SentDateTimeOutOfRange.Description,
                    value));
        });        
    }
}