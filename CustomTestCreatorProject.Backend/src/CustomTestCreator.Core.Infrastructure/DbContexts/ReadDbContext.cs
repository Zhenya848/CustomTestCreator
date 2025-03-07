using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.Core.Application.Repositories;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CustomTestCreator.Core.Infrastructure.DbContexts;

public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    public IQueryable<ClientDto> Clients => Set<ClientDto>();
    public IQueryable<TestDto> Tests => Set<TestDto>();
    public IQueryable<TaskDto> Tasks => Set<TaskDto>();
    public IQueryable<TestHistoryDto> TestHistories => Set<TestHistoryDto>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReadDbContext).Assembly); 

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(b => b.AddConsole());
}