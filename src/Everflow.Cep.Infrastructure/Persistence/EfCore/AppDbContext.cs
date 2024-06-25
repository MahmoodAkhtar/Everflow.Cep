using Everflow.Cep.Core.Events;
using Everflow.Cep.Core.Invitations;
using Everflow.Cep.Core.Users;
using Microsoft.EntityFrameworkCore;

namespace Everflow.Cep.Infrastructure.Persistence.EfCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Invitation> Invitations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasData(new List<User>()
            {
                new User(1, "User One", "user1@example.com", "P@ssword1"),
                new User(2, "User Two", "user2@example.com", "P@ssword2"),
                new User(3, "User Three", "user3@example.com", "P@ssword3"),
            });

        modelBuilder.Entity<Event>()
            .HasData(
                new
                {
                    Id = 1, CreatedByUserId = 1, Name = "Event One", Description = "Event One description",
                    StartDateTime = DateTime.UtcNow.AddDays(-3), EndDateTime = DateTime.UtcNow.AddDays(-2),
                    Status = EventStatuses.Finished
                },
                new
                {
                    Id = 2, CreatedByUserId = 1, Name = "Event Two", Description = "Event Two description",
                    StartDateTime = DateTime.UtcNow.AddHours(-10), EndDateTime = DateTime.UtcNow.AddHours(10),
                    Status = EventStatuses.CloseToInvitation
                },
                new
                {
                    Id = 3, CreatedByUserId = 1, Name = "Event Three", Description = "Event Three description",
                    StartDateTime = DateTime.UtcNow.AddHours(-5), EndDateTime = DateTime.UtcNow.AddHours(2),
                    Status = EventStatuses.OpenToInvitation
                },
                new
                {
                    Id = 4, CreatedByUserId = 1, Name = "Event Four", Description = "Event Four description",
                    StartDateTime = DateTime.UtcNow.AddHours(1), EndDateTime = DateTime.UtcNow.AddHours(5),
                    Status = EventStatuses.Draft
                });

        modelBuilder.Entity<Event>()
            .HasOne<User>(x => x.CreatedByUser);

        modelBuilder.Entity<Event>()
            .Property(x => x.Status)
            .HasConversion(
                x => x.GetType().Name,
                x => (EventStatus)Activator
                    .CreateInstance(
                        typeof(EventStatus).Assembly.FullName, 
                        $"{typeof(EventStatus).FullName!
                            .Remove(typeof(EventStatus).FullName!
                                .LastIndexOf('.'))}.{x}")
                    .Unwrap());
        
        modelBuilder.Entity<Invitation>()
            .HasOne<User>(x => x.InvitedUser)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Invitation>()
            .HasOne<Event>(x => x.InvitedToEvent)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Invitation>()
            .Property(x => x.ResponseStatus)
            .HasConversion(
                x => x.GetType().Name,
                x => (InvitationResponseStatus)Activator
                    .CreateInstance(
                        typeof(InvitationResponseStatus).Assembly.FullName, 
                        $"{typeof(InvitationResponseStatus).FullName!
                            .Remove(typeof(InvitationResponseStatus).FullName!
                                .LastIndexOf('.'))}.{x}")
                    .Unwrap());
    }
}