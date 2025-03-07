using CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;

namespace CustomTestCreator.Core.Application.Repositories;

public interface IReadDbContext
{
    public IQueryable<ClientDto> Clients { get; }
    public IQueryable<TestDto> Tests { get; }
    public IQueryable<TaskDto> Tasks { get; }
    public IQueryable<TestHistoryDto> TestHistories { get; }
}