using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Events.List;

public class ListEventStatusesHandler : IRequestHandler<ListEventStatusesQuery, Result<IEnumerable<EventStatusDto>, Error>>
{
    private readonly IValidator<ListEventStatusesQuery> _validator;

    public ListEventStatusesHandler(IValidator<ListEventStatusesQuery> validator)
    {
        _validator = validator;
    }
    
    public async Task<Result<IEnumerable<EventStatusDto>, Error>> Handle(ListEventStatusesQuery request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return await Task.FromResult(validationResult.Error);

        return new EventStatusDto[]
        {
            EventStatusDtos.Draft, 
            EventStatusDtos.Finished, 
            EventStatusDtos.CloseToInvitation, 
            EventStatusDtos.OpenToInvitation, 
        };
    }
}