using Everflow.Cep.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Everflow.Cep.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<OffsetPaginationSettings>(configuration.GetSection(nameof(OffsetPaginationSettings))
            .Get<OffsetPaginationSettings>());

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserRulesService, UserRulesService>();
        services.AddScoped<IInvitationRulesService, InvitationRulesService>();

        return services;
    }
}