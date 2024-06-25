using Everflow.SharedKernal;

namespace Everflow.Cep.Application.Invitations.List;

public class InvitationResponseStatusesQueryValidator : IValidator<InvitationResponseStatusesQuery>
{
    public Result<bool, Error> Validate(InvitationResponseStatusesQuery value)
    {
        var errors = new List<Error>();
        if (value is null) errors.Add(InvitationResponseStatusesQuery.Errors.IsNull);
        
        return errors.Count is 0 ? true : errors.First();
    }
}