using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Users;
using Everflow.Cep.Infrastructure.Persistence.EfCore;
using Everflow.SharedKernal;
using Microsoft.EntityFrameworkCore;

namespace Everflow.Cep.Infrastructure.Persistence;

public class UsersRepository(IAppDbContext context) : IUsersRepository
{
    public async Task<Result<int, Error>> AddAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    public async Task<Result<bool, Error>> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            var isUnique = !await context.Users.AnyAsync(x => x.Email == email, cancellationToken);

            return isUnique;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    public async Task<Result<User?, Error>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            var found = await context.Users.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);

            return found;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    public async Task<Result<User?, Error>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var user = await context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            return user;
        }
        catch (Exception e)
        {
            return Error.FromException(e);;
        }
    }
    
    public async Task<Result<IEnumerable<User>, Error>> ListAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        try
        {
            var users = await context.Users.AsNoTracking()
                .Skip(offset)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return users;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    public async Task<Result<User, Error>> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            var found = await context.Users.AsTracking().SingleOrDefaultAsync(x => x.Id == user.Id, cancellationToken);
            if (found is null) return User.Errors.NotFound(user.Id);

            found.SetName(user.Name);
            found.SetEmail(user.Email);
            found.SetPassword(user.Password);

            await context.SaveChangesAsync(cancellationToken);

            return found;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    public async Task<Result<bool, Error>> DeleteByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var found = await context.Users.AsTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (found is null) return User.Errors.NotFound(id);

            context.Users.Remove(found);

            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }
}