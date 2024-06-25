using Everflow.Cep.Core.Invitations;
using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Interfaces;

public interface IInvitationsRepository
{
    Task<Result<int, Error>> AddAsync(Invitation invitation, CancellationToken cancellationToken);
    Task<Result<Invitation?, Error>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<IEnumerable<Invitation>, Error>> ListAsync(int offset, int limit, CancellationToken cancellationToken);
    Task<Result<Invitation, Error>> UpdateAsync(Invitation invitation, CancellationToken cancellationToken);
    Task<Result<bool, Error>> DeleteByIdAsync(int id, CancellationToken cancellationToken);
}