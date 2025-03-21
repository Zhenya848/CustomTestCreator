using CustomTestCreator.Accounts.Application;
using CustomTestCreator.Accounts.Application.Repositories;
using CustomTestCreator.Accounts.Domain.User;
using CustomTestCreator.Accounts.Infrastructure.DbContexts;
using CustomTestCreator.Accounts.Infrastructure.Repositories;
using CustomTestCreator.Accounts.Infrastructure.Seeding;
using CustomTestCreator.Accounts.Presentation;
using CustomTestCreator.Accounts.Presentation.Options;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.SharedKernel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Accounts.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddFromAccountsInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAccountsIdentity()
            .AddAuthenticationWithJwtTokens(configuration);
        
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Accounts);
        
        services.AddTransient<ITokenProvider, JwtTokenProvider>();

        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();

        return services;
    }

    private static IServiceCollection AddAccountsIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<AccountDbContext>();
        
        return services.AddScoped<AccountDbContext>();
    }
    
    private static IServiceCollection AddAuthenticationWithJwtTokens(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.JWT));
        
        services.AddOptions<JwtOptions>();
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                                 ?? throw new ApplicationException("Missing JWT configuration");

                options.TokenValidationParameters = TokenValidationParametersFactory
                    .CreateWithLifeTime(jwtOptions);
            });
        
        services.AddAuthentication();

        services.AddAuthorization();
        
        return services;
    }
}