using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Services;

public interface IUserRulesService
{
    Task<Result<bool, Error>> CheckIsEmailUniqueAsync(string email, CancellationToken cancellationToken);
    Task<Result<bool, Error>> CheckIsEmailUniqueForIdAsync(int id, string email, CancellationToken cancellationToken);
    Task<Result<bool, Error>> CheckShouldUserBeDeletedAsync(int id, CancellationToken cancellationToken);
}