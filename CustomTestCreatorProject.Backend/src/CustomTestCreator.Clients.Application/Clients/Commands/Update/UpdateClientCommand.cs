using CustomTestCreator.SharedKernel.ValueObjects.Dtos;

namespace CustomTestCreator.Clients.Application.Clients.Commands.Update;

public record UpdateClientCommand(
    Guid ClientId,
    string Name,
    bool IsRandomTasks,
    bool IsInfiniteMode);