using CustomTestCreator.Clients.Application.Providers;
using CustomTestCreator.Clients.Application.Repositories;
using CustomTestCreator.Clients.Infrastructure.BackgroundServices;
using CustomTestCreator.Clients.Infrastructure.DbContexts;
using CustomTestCreator.Clients.Infrastructure.Options;
using CustomTestCreator.Clients.Infrastructure.Providers;
using CustomTestCreator.Clients.Infrastructure.Repositories;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.Core.Application.Messaging;
using CustomTestCreator.Core.Infrastructure.MessageQueue;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using FileInfo = CustomTestCreator.SharedKernel.ValueObjects.FileInfo;

namespace CustomTestCreator.Clients.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddFromClientsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ClientDbContext>();
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Client);
        services.AddScoped<IClientRepository, ClientRepository>();

        services.AddHostedService<DeleteExpiredClientsBackgroundService>();
        services.AddHostedService<FileCleanerBackgroundService>();
        services.AddMinio(configuration);

        services.AddScoped<IFileProvider, MinioProvider>();

        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>,
            InMemoryMessageQueue<IEnumerable<FileInfo>>>();
        
        return services;
    }

    private static IServiceCollection AddMinio(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddMinio(o =>
        {
            MinioOptions minioOptions = configuration.GetSection("Minio").Get<MinioOptions>()
                                        ?? throw new ApplicationException("Minio options is missing.");

            o.WithEndpoint(minioOptions.Endpoint);
            o.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);

            o.WithSSL(minioOptions.WithSsl);
        });
    }
}