using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Invitations;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Invitations.Get;

public class GetInvitationByIdHandler : IRequestHandler<GetInvitationByIdQuery, Result<InvitationDto?, Error>>
{
    private readonly IInvitationsRepository _invitationsRepository;
    private readonly IValidator<GetInvitationByIdQuery> _validator;

    public GetInvitationByIdHandler(
        IInvitationsRepository invitationsRepository,
        IValidator<GetInvitationByIdQuery> validator)
    {
        _invitationsRepository = invitationsRepository;
        _validator = validator;
    }

    public async Task<Result<InvitationDto?, Error>> Handle(GetInvitationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var getByIdResult = await _invitationsRepository.GetByIdAsync(request.Id, cancellationToken);

        return getByIdResult.Match<Result<InvitationDto?, Error>>(
            invitation => invitation is null
                ? Invitation.Errors.NotFound(request.Id)
                : InvitationDto.FromInvitation(invitation),
            error => error);
    }
}