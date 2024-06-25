using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Interfaces;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Events.Get;

public class GetEventByIdHandler : IRequestHandler<GetEventByIdQuery, Result<EventDto?, Error>>
{
    private readonly IEventsRepository _eventRepository;
    private readonly IValidator<GetEventByIdQuery> _validator;

    public GetEventByIdHandler(IEventsRepository eventRepository, IValidator<GetEventByIdQuery> validator)
    {
        _eventRepository = eventRepository;
        _validator = validator;
    }

    public async Task<Result<EventDto?, Error>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var getByIdResult = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

        return getByIdResult.Match<Result<EventDto?, Error>>(
            @event => @event is null
                ? Event.Errors.NotFound(request.Id)
                : EventDto.FromEvent(@event),
            error => error);
    }
}