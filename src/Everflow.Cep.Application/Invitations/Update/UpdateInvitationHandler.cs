using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Invitations;
using Everflow.Cep.Core.Services;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Invitations.Update;

public class UpdateInvitationHandler : IRequestHandler<UpdateInvitationCommand, Result<InvitationDto, Error>>
{
    private readonly IInvitationsRepository _invitationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IEventsRepository _eventsRepository;
    private readonly IValidator<UpdateInvitationCommand> _validator;
    private readonly IInvitationRulesService _invitationRulesService;

    public UpdateInvitationHandler(
        IInvitationsRepository invitationsRepository,
        IUsersRepository usersRepository,
        IEventsRepository eventsRepository,
        IValidator<UpdateInvitationCommand> validator,
        IInvitationRulesService invitationRulesService)
    {
        _invitationsRepository = invitationsRepository;
        _usersRepository = usersRepository;
        _eventsRepository = eventsRepository;
        _validator = validator;
        _invitationRulesService = invitationRulesService;
    }

    public async Task<Result<InvitationDto, Error>> Handle(
        UpdateInvitationCommand request, 
        CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var userResult = await _usersRepository.GetByIdAsync(request.InvitedUserId, cancellationToken);
        if (userResult.IsFailure) return userResult.Error;

        var eventResult = await _eventsRepository.GetByIdAsync(request.InvitedToEventId, cancellationToken);
        if (eventResult.IsFailure) return eventResult.Error;

        var invitationResult = await _invitationsRepository.GetByIdAsync(request.Id, cancellationToken);
        if (invitationResult.IsFailure) return invitationResult.Error;        
        
        var checkResult = _invitationRulesService.CheckIsUserInvitableToEvent(
            userResult.Value!, 
            invitationResult.Value!, 
            eventResult.Value!);
        
        if (checkResult.IsFailure) return checkResult.Error;
        
        var updateResult = await _invitationsRepository
            .UpdateAsync(
                new Invitation(
                    request.Id, 
                    userResult.Value!, 
                    eventResult.Value!, 
                    request.SentDateTime,
                    InvitationResponseStatuses.FromString(request.ResponseStatus.GetType().Name)),
                cancellationToken);

        return updateResult.Match<Result<InvitationDto, Error>>(
            invitation => InvitationDto.FromInvitation(invitation),
            error => error);
    }
}