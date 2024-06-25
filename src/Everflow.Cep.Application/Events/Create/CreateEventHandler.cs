using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Interfaces;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Events.Create;

public class CreateEventHandler : IRequestHandler<CreateEventCommand, Result<int, Error>>
{
    private readonly IEventsRepository _eventRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IValidator<CreateEventCommand> _validator;

    public CreateEventHandler(
        IEventsRepository eventRepository, 
        IUsersRepository usersRepository, 
        IValidator<CreateEventCommand> validator)
    {
        _eventRepository = eventRepository;
        _usersRepository = usersRepository;
        _validator = validator;
    }

    public async Task<Result<int, Error>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var userResult = await _usersRepository.GetByIdAsync(request.CreatedByUserId, cancellationToken);
        if (userResult.IsFailure) return userResult.Error;
        
        var addResult = await _eventRepository.AddAsync(
            new Event(userResult.Value!, request.Name, request.Description, request.StartDateTime,
                request.EndDateTime, EventStatuses.Draft), cancellationToken);

        return addResult;
    }
}