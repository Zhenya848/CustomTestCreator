using CustomTestCreator.Clients.Domain;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;

namespace CustomTestCreator.Clients.Application.Tasks.Commands.Create;

public record CreateTasksCommand(
    Guid ClientId,
    Guid TestId,
    IEnumerable<CreateTaskDto> Tasks);