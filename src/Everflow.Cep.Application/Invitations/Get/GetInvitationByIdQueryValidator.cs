using Everflow.Cep.Core.Invitations;
using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Invitations.Get;

public class GetInvitationByIdQueryValidator : IValidator<GetInvitationByIdQuery>
{
    public Result<bool, Error> Validate(GetInvitationByIdQuery value)
    {
        var arg = () => value is null
            ? GetInvitationByIdQuery.Errors.IsNull
            : Error.None;

        var id = () =>
            new Invitation.IdSpecification().IsSatisfiedBy(value.Id)
                ? Error.None
                : Invitation.Errors.IdMustBeGreaterThanZero;

        var errors = new[] { arg, id }
            .Select(func => func())
            .Where(error => error != Error.None)
            .ToList();

        return errors.Count is 0 ? true : errors.First();
    }
}