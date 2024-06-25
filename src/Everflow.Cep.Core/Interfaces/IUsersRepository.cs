using Everflow.SharedKernal;
using Everflow.Cep.Core.Users;

namespace Everflow.Cep.Core.Interfaces;

public interface IUsersRepository
{
    Task<Result<int, Error>> AddAsync(User user, CancellationToken cancellationToken);
    Task<Result<User?, Error>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<IEnumerable<User>, Error>> ListAsync(int offset, int limit, CancellationToken cancellationToken);
    Task<Result<User, Error>> UpdateAsync(User user, CancellationToken cancellationToken);
    Task<Result<bool, Error>> DeleteByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<bool, Error>> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);
    Task<Result<User?, Error>> GetByEmailAsync(string email, CancellationToken cancellationToken);
}