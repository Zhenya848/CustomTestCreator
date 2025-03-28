using CSharpFunctionalExtensions;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects.Id;

namespace CustomTestCreator.Clients.Contracts;

public interface IClientsContract
{
    Task CreateClient(
        ClientId clientId,
        string name,
        bool isRandomTasks,
        bool isInfiniteMode,
        CancellationToken cancellationToken = default);
}