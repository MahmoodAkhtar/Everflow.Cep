using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Invitations;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Services;

public interface IInvitationRulesService
{
    Result<bool, Error> CheckIsUserInvitableToEvent(
        User user, 
        Invitation invitation, 
        Event @event);
}