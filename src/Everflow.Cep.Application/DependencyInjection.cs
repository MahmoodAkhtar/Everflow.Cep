using Everflow.SharedKernal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Everflow.Cep.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        // Register IValidator<> ... can this be neatened up ?
        System.Reflection.Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(item => item.GetInterfaces()
                .Where(i => i.IsGenericType)
                .Any(i => 
                    i.GetGenericTypeDefinition() == typeof(IValidator<>)) 
                           && !item.IsAbstract 
                           && !item.IsInterface)
            .ToList()
            .ForEach(assignedTypes =>
            {
                var serviceType = assignedTypes.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IValidator<>));
                services.AddTransient(serviceType, assignedTypes);
            });

        return services;
    }
}