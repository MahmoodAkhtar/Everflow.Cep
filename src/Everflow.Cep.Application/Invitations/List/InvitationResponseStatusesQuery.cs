using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Invitations.List;

public record InvitationResponseStatusesQuery : IRequest<Result<IEnumerable<InvitationResponseStatusDto>, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("InvitationResponseStatusesQuery.IsNull", "InvitationResponseStatusesQuery is null");
    }
}