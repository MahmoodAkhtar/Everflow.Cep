using Everflow.Cep.Core.Events;
using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Interfaces;

public interface IEventsRepository
{
    Task<Result<int, Error>> AddAsync(Event @event, CancellationToken cancellationToken);
    Task<Result<Event?, Error>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<IEnumerable<Event>, Error>> ListAsync(int offset, int limit, CancellationToken cancellationToken);
    Task<Result<Event, Error>> UpdateAsync(Event @event, CancellationToken cancellationToken);
    Task<Result<bool, Error>> DeleteByIdAsync(int id, CancellationToken cancellationToken);
}