using CustomTestCreator.Clients.Domain.ValueObjects;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos;

namespace CustomTestCreator.Clients.Application.Clients.Commands.Create;

public record CreateClientCommand(
    string Name,
    bool IsRandomTasks,
    bool IsInfiniteMode);