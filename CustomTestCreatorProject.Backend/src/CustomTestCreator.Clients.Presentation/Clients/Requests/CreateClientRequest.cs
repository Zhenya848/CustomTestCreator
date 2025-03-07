using CustomTestCreator.Clients.Domain.ValueObjects;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos;

namespace CustomTestCreator.Clients.Presentation.Clients.Requests;

public record CreateClientRequest(
    string Name,
    bool IsRandomTasks,
    bool IsInfiniteMode);