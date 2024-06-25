using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Infrastructure.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Everflow.Cep.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection"), 
            ob => ob.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IAppDbContext, AppDbContext>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IEventsRepository, EventsRepository>();
        services.AddScoped<IInvitationsRepository, InvitationsRepository>();
        
        return services;
    }
}