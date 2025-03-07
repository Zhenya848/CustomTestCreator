namespace CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;

public record ClientDto
{
    public Guid Id { get; }
    public string Name { get; }

    public bool IsRandomTasks { get; }
    public bool IsInfiniteMode { get; }
}