using CustomTestCreator.Clients.Infrastructure.DbContexts;
using CustomTestCreator.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CustomTestCreator.Clients.Infrastructure.BackgroundServices;

public class DeleteExpiredClientsBackgroundService : BackgroundService
{
    private readonly ILogger<DeleteExpiredClientsBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public DeleteExpiredClientsBackgroundService(
        ILogger<DeleteExpiredClientsBackgroundService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DeleteExpiredClientsBackgroundService is starting.");

        while (stoppingToken.IsCancellationRequested == false)
        {
            using var clients = _scopeFactory.CreateScope();
            var writeDbContext = clients.ServiceProvider.GetRequiredService<ClientDbContext>();
            
            var clientsToDelete = writeDbContext.Clients
                .Include(t => t.Tests)
                .Where(dt => dt.IsDeleted && DateTime.UtcNow >= dt.DeletionDate
                    .AddDays(Constants.Clients.LIFE_AFTER_DELETION_IN_DAYS));
            
            writeDbContext.Clients.RemoveRange(clientsToDelete);
            await writeDbContext.SaveChangesAsync(stoppingToken);
            
            await Task.Delay(
                TimeSpan.FromHours(Constants.Clients.DELETE_EXPIRED_VOLUNTEERS_SERVICE_REDUCTION_HOURS), 
                stoppingToken);
        }
    }
}