using Everflow.Cep.Core.Invitations;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Invitations.Delete;

public class DeleteInvitationByIdCommandValidator: IValidator<DeleteInvitationByIdCommand>
{
    public Result<bool, Error> Validate(DeleteInvitationByIdCommand value)
    {
        var arg = () => value is null ? DeleteInvitationByIdCommand.Errors.IsNull : Error.None;
        
        var id = () => new Invitation.IdSpecification().IsSatisfiedBy(value.Id) 
            ? Error.None 
            : Invitation.Errors.IdMustBeGreaterThanZero;

        var errors = new[] { arg, id }
            .Select(func => func())
            .Where(error => error != Error.None)
            .ToList();
        
        return errors.Count is 0 ? true : errors.First();
    }
}