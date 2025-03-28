using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Application.Clients.Commands.Create;
using CustomTestCreator.Clients.Application.Repositories;
using CustomTestCreator.Clients.Contracts;
using CustomTestCreator.Clients.Domain;
using CustomTestCreator.Clients.Domain.ValueObjects;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.Extensions.DependencyInjection;
using Task = System.Threading.Tasks.Task;

namespace CustomTestCreator.Clients.Implementation;

public class ClientsContract : IClientsContract
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClientsContract(
        IClientRepository clientRepository,
        [FromKeyedServices(Modules.Client)]IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task CreateClient(
        ClientId clientId,
        string name,
        bool isRandomTasks,
        bool isInfiniteMode,
        CancellationToken cancellationToken = default)
    {
        var taskSettings = new TaskSettings(isRandomTasks, isInfiniteMode);
        Client client = new Client(clientId, name, taskSettings);
        
        _clientRepository.Add(client);
        await _unitOfWork.SaveChanges(cancellationToken);
    }
}