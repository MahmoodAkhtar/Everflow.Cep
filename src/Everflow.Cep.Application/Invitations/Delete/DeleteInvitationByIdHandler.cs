using Everflow.Cep.Core.Interfaces;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Invitations.Delete;


public class DeleteInvitationByIdHandler : IRequestHandler<DeleteInvitationByIdCommand, Result<bool, Error>>
{
    private readonly IInvitationsRepository _invitationsRepository;
    private readonly IValidator<DeleteInvitationByIdCommand> _validator;

    public DeleteInvitationByIdHandler(IInvitationsRepository invitationsRepository, IValidator<DeleteInvitationByIdCommand> validator)
    {
        _invitationsRepository = invitationsRepository;
        _validator = validator;
    }
    
    public async Task<Result<bool, Error>> Handle(DeleteInvitationByIdCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var deleteResult = await _invitationsRepository.DeleteByIdAsync(request.Id, cancellationToken);
        
        return deleteResult;
    }
}