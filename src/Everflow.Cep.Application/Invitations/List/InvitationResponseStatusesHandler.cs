using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Invitations.List;

public class InvitationResponseStatusesHandler : IRequestHandler<InvitationResponseStatusesQuery, Result<IEnumerable<InvitationResponseStatusDto>, Error>>
{
    private readonly IValidator<InvitationResponseStatusesQuery> _validator;

    public InvitationResponseStatusesHandler(IValidator<InvitationResponseStatusesQuery> validator)
    {
        _validator = validator;
    }
    
    public async Task<Result<IEnumerable<InvitationResponseStatusDto>, Error>> Handle(InvitationResponseStatusesQuery request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return await Task.FromResult(validationResult.Error);

        return new InvitationResponseStatusDto[]
        {
            InvitationResponseStatusDtos.NoReply,
            InvitationResponseStatusDtos.Accept,
            InvitationResponseStatusDtos.Maybe,
            InvitationResponseStatusDtos.Reject,
        };
    }
}