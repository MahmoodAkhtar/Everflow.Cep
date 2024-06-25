using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Invitations;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Services;

public class InvitationRulesService : IInvitationRulesService
{
    public Result<bool, Error> CheckIsUserInvitableToEvent(
        User user, 
        Invitation invitation, 
        Event @event)
    {
        if (user is null) 
            return User.Errors.IsNull;
        
        if (invitation is null) 
            return Invitation.Errors.IsNull;
        
        if (@event is null) 
            return Event.Errors.IsNull;
       
        if (@event.Status == EventStatuses.OpenToInvitation)
            return Invitation.Errors.InvitedToEventStatusMustBeOpenForInvitation;

        if (user.Id == @event.CreatedByUser.Id)
            return Invitation.Errors.InvitedUserCannotBeInvitedToEventCreatedBySameUser;

        return true;
    }
}