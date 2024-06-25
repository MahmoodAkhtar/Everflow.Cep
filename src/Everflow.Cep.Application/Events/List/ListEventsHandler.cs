using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Services;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Events.List;

public class ListEventsHandler : IRequestHandler<ListEventsQuery, Result<IEnumerable<EventDto>, Error>>
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IValidator<ListEventsQuery> _validator;
    private readonly OffsetPaginationSettings _offsetPaginationSettings;

    public ListEventsHandler(
        IEventsRepository eventsRepository, 
        IValidator<ListEventsQuery> validator,
        OffsetPaginationSettings offsetPaginationSettings)
    {
        _eventsRepository = eventsRepository;
        _validator = validator;
        _offsetPaginationSettings = offsetPaginationSettings;
    }

    public async Task<Result<IEnumerable<EventDto>, Error>> Handle(ListEventsQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var offsetLimit = OffsetPagination.Create(request.Offset, request.Limit, _offsetPaginationSettings);
        var listResult = await _eventsRepository.ListAsync(offsetLimit.Offset, offsetLimit.Limit, cancellationToken);

        return listResult.Match<Result<IEnumerable<EventDto>, Error>>(
            users => users.Select(EventDto.FromEvent).ToList(),
            error => error);
    }
}