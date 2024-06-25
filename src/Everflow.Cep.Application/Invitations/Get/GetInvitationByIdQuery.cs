using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Invitations.Get;

public record GetInvitationByIdQuery(int Id) : IRequest<Result<InvitationDto?, Error>>
{
    public static class Errors
    {
        public static Error IsNull => Error.Conflict("GetInvitationByIdQuery.IsNull", "GetInvitationByIdQuery is null");
    }
}