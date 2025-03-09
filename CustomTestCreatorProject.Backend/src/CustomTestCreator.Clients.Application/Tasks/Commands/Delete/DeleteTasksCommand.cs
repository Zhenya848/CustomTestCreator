namespace CustomTestCreator.Clients.Application.Tasks.Commands.Delete;

public record DeleteTasksCommand(
    Guid ClientId,
    Guid TestId,
    IEnumerable<Guid> TasIds);