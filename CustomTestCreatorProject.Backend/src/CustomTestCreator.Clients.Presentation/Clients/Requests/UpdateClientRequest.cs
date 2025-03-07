using CustomTestCreator.Clients.Domain.ValueObjects;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos;

namespace CustomTestCreator.Clients.Presentation.Clients.Requests;

public record UpdateClientRequest(
    string Name,
    bool IsRandomTasks,
    bool IsInfiniteMode);