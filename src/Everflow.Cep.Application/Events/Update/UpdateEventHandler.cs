using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Interfaces;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Events.Update;

public class UpdateEventHandler : IRequestHandler<UpdateEventCommand, Result<EventDto, Error>>
{
    private readonly IEventsRepository _eventRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IValidator<UpdateEventCommand> _validator;

    public UpdateEventHandler(IEventsRepository eventRepository, IUsersRepository usersRepository, IValidator<UpdateEventCommand> validator)
    {
        _eventRepository = eventRepository;
        _usersRepository = usersRepository;
        _validator = validator;
    }

    public async Task<Result<EventDto, Error>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var userResult = await _usersRepository.GetByIdAsync(request.CreatedByUserId, cancellationToken);
        if (userResult.IsFailure) return userResult.Error;

        var updateResult = await _eventRepository
            .UpdateAsync(new Event(request.Id, userResult.Value!, request.Name, request.Description, request.StartDateTime,
                request.EndDateTime, 
                EventStatuses.FromString(request.Status.GetType().Name)), cancellationToken);

        return updateResult.Match<Result<EventDto, Error>>(
            @event => EventDto.FromEvent(@event),
            error => error);
    }
}