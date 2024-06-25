using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Invitations.List;

public record ListInvitationsQuery(int Offset, int Limit) : IRequest<Result<IEnumerable<InvitationDto>, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("ListInvitationsQuery.IsNull", "ListInvitationsQuery is null");
    }
}