using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Invitations;
using Everflow.Cep.Core.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Everflow.Cep.Infrastructure.Persistence.EfCore;

public interface IAppDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Event> Events { get; set; }
    DbSet<Invitation> Invitations { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    EntityEntry Entry(object entity);
}