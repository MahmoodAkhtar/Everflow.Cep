using Everflow.Cep.Core.Interfaces;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Events.Delete;

public class DeleteEventByIdHandler : IRequestHandler<DeleteEventByIdCommand, Result<bool, Error>>
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IValidator<DeleteEventByIdCommand> _validator;

    public DeleteEventByIdHandler(IEventsRepository eventsRepository, IValidator<DeleteEventByIdCommand> validator)
    {
        _eventsRepository = eventsRepository;
        _validator = validator;
    }
    
    public async Task<Result<bool, Error>> Handle(DeleteEventByIdCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var deleteResult = await _eventsRepository.DeleteByIdAsync(request.Id, cancellationToken);
        
        return deleteResult;
    }
}