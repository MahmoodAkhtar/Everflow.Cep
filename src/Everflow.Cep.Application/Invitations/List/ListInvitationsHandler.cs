using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Services;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Invitations.List;


public class ListInvitationsHandler : IRequestHandler<ListInvitationsQuery, Result<IEnumerable<InvitationDto>, Error>>
{
    private readonly IInvitationsRepository _invitationsRepository;
    private readonly IValidator<ListInvitationsQuery> _validator;
    private readonly OffsetPaginationSettings _offsetPaginationSettings;

    public ListInvitationsHandler(
        IInvitationsRepository invitationsRepository, 
        IValidator<ListInvitationsQuery> validator,
        OffsetPaginationSettings offsetPaginationSettings)
    {
        _invitationsRepository = invitationsRepository;
        _validator = validator;
        _offsetPaginationSettings = offsetPaginationSettings;
    }

    public async Task<Result<IEnumerable<InvitationDto>, Error>> Handle(ListInvitationsQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var offsetLimit = OffsetPagination.Create(request.Offset, request.Limit, _offsetPaginationSettings);
        var listResult = await _invitationsRepository.ListAsync(offsetLimit.Offset, offsetLimit.Limit, cancellationToken);

        return listResult.Match<Result<IEnumerable<InvitationDto>, Error>>(
            users => users.Select(InvitationDto.FromInvitation).ToList(),
            error => error);
    }
}