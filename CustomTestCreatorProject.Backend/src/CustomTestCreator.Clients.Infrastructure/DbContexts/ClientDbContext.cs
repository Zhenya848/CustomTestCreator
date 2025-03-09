using CustomTestCreator.Clients.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Task = CustomTestCreator.Clients.Domain.Task;

namespace CustomTestCreator.Clients.Infrastructure.DbContexts;

public class ClientDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Test> Tests => Set<Test>();
    public DbSet<Task> Tasks => Set<Task>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
       modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientDbContext).Assembly); 

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(b => b.AddConsole());
}