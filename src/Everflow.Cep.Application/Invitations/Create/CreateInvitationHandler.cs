using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Invitations;
using Everflow.Cep.Core.Services;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Invitations.Create;

public class CreateInvitationHandler : IRequestHandler<CreateInvitationCommand, Result<int, Error>>
{
    private readonly IInvitationsRepository _invitationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IEventsRepository _eventsRepository;
    private readonly IValidator<CreateInvitationCommand> _validator;
    private readonly IInvitationRulesService _invitationRulesService;

    public CreateInvitationHandler(
        IInvitationsRepository invitationsRepository,
        IUsersRepository usersRepository,
        IEventsRepository eventsRepository,
        IValidator<CreateInvitationCommand> validator,
        IInvitationRulesService invitationRulesService)
    {
        _invitationsRepository = invitationsRepository;
        _usersRepository = usersRepository;
        _eventsRepository = eventsRepository;
        _validator = validator;
        _invitationRulesService = invitationRulesService;
    }

    public async Task<Result<int, Error>> Handle(
        CreateInvitationCommand request, 
        CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var userResult = await _usersRepository.GetByIdAsync(request.InvitedUserId, cancellationToken);
        if (userResult.IsFailure) return userResult.Error;

        var eventResult = await _eventsRepository.GetByIdAsync(request.InvitedToEventId, cancellationToken);
        if (eventResult.IsFailure) return eventResult.Error;

        var invitation = new Invitation(
            userResult.Value!,
            eventResult.Value!,
            request.SentDateTime,
            InvitationResponseStatuses.NoReply);
        
        var checkResult = _invitationRulesService.CheckIsUserInvitableToEvent(
            userResult.Value!, 
            invitation, 
            eventResult.Value!);
        
        var addResult = await _invitationsRepository.AddAsync(invitation, cancellationToken);

        return addResult;
    }
}