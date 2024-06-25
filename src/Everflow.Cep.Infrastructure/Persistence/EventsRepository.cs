using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Infrastructure.Persistence.EfCore;
using Everflow.SharedKernal;
using Microsoft.EntityFrameworkCore;

namespace Everflow.Cep.Infrastructure.Persistence;

public class EventsRepository(IAppDbContext context) : IEventsRepository
{
    public async Task<Result<int, Error>> AddAsync(Event @event, CancellationToken cancellationToken)
    {
        try
        {
            context.Entry(@event.CreatedByUser).State = EntityState.Unchanged;
            await context.Events.AddAsync(@event, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return @event.Id;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    public async Task<Result<Event?, Error>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var @event = await context.Events.AsNoTracking()
                .Include(x => x.CreatedByUser)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            return @event;
        }
        catch (Exception e)
        {
            return Error.FromException(e);;
        }
    }
    
    public async Task<Result<IEnumerable<Event>, Error>> ListAsync(
        int offset, int limit, CancellationToken cancellationToken)
    {
        try
        {
            var @events = await context.Events.AsNoTracking()
                .Include(x => x.CreatedByUser)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return @events;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    public async Task<Result<Event, Error>> UpdateAsync(Event @event, CancellationToken cancellationToken)
    {
        try
        {
            var found = await context.Events
                .AsTracking()
                .SingleOrDefaultAsync(x => x.Id == @event.Id, cancellationToken);
            
            if (found is null) return Event.Errors.NotFound(@event.Id);

            found.SetCreatedByUser(@event.CreatedByUser);
            found.SetName(@event.Name);
            found.SetDescription(@event.Description);
            found.SetStartDateTimeAndEndDateTime(@event.StartDateTime, @event.EndDateTime);
            found.SetStatus(@event.Status);

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
            var found = await context.Events
                .AsTracking()
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            
            if (found is null) return Event.Errors.NotFound(id);

            context.Events.Remove(found);

            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }
}