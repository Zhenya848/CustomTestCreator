using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Domain;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Task = CustomTestCreator.Clients.Domain.Task;

namespace CustomTestCreator.Clients.Application.Repositories;

public interface IClientRepository
{
    public Guid Add(Client client);
    
    public Guid Save(Client client);
    
    public Task<Result<Client, ErrorList>> GetById(
        ClientId id,
        CancellationToken cancellationToken);
    
    public Guid DeleteTest(Test test);
    
    public IEnumerable<Guid> DeleteTasks(IEnumerable<Task> tasks);
}