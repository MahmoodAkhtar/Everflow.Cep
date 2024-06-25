using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Invitations;
using Everflow.Cep.Infrastructure.Persistence.EfCore;
using Everflow.SharedKernal;
using Microsoft.EntityFrameworkCore;

namespace Everflow.Cep.Infrastructure.Persistence;

public class InvitationsRepository(IAppDbContext context) : IInvitationsRepository
{
    public async Task<Result<int, Error>> AddAsync(Invitation invitation, CancellationToken cancellationToken)
    {
        try
        {
            context.Entry(invitation.InvitedUser).State = EntityState.Unchanged;
            context.Entry(invitation.InvitedToEvent).State = EntityState.Unchanged;
            await context.Invitations.AddAsync(invitation, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return invitation.Id;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    public async Task<Result<Invitation?, Error>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var invitation = await context.Invitations.AsNoTracking()
                .Include(x => x.InvitedUser)
                .Include(x => x.InvitedToEvent)
                .ThenInclude(x => x.CreatedByUser)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            return invitation;
        }
        catch (Exception e)
        {
            return Error.FromException(e);;
        }
    }

    public async Task<Result<IEnumerable<Invitation>, Error>> ListAsync(
        int offset, int limit, CancellationToken cancellationToken)
    {
        try
        {
            var invitations = await context.Invitations.AsNoTracking()
                .Include(x => x.InvitedUser)
                .Include(x => x.InvitedToEvent)
                .ThenInclude(x => x.CreatedByUser)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return invitations;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    public async Task<Result<Invitation, Error>> UpdateAsync(Invitation invitation, CancellationToken cancellationToken)
    {
        try
        {
            var found = await context.Invitations
                .AsTracking()
                .SingleOrDefaultAsync(x => x.Id == invitation.Id, cancellationToken);
            
            if (found is null) return Invitation.Errors.NotFound(invitation.Id);

            found.SetInvitedUserAndInvitedToEvent(invitation.InvitedUser, invitation.InvitedToEvent);
            found.SetSentDateTime(invitation.SentDateTime);
            found.SetResponseStatus(invitation.ResponseStatus);

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
            var found = await context.Invitations
                .AsTracking()
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            
            if (found is null) return Invitation.Errors.NotFound(id);

            context.Invitations.Remove(found);

            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }
}