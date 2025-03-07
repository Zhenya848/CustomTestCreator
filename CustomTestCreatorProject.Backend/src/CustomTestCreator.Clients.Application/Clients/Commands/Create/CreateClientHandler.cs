using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Application.Repositories;
using CustomTestCreator.Clients.Domain;
using CustomTestCreator.Clients.Domain.ValueObjects;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.Core.Application.Validation;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Clients.Application.Clients.Commands.Create;

public class CreateClientHandler : ICommandHandler<CreateClientCommand, Result<Guid, ErrorList>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientRepository _clientRepository;

    public CreateClientHandler(
        [FromKeyedServices(Modules.Client)]IUnitOfWork unitOfWork, 
        IClientRepository clientRepository)
    {
        _unitOfWork = unitOfWork;
        _clientRepository = clientRepository;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateClientCommand command, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return (ErrorList)Errors.General.ValueIsRequired("Name");

        var taskSettings = new TaskSettings(command.IsRandomTasks, command.IsInfiniteMode);
        Client client = new Client(ClientId.AddNewId(), command.Name, taskSettings);
        
        _clientRepository.Add(client);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return (Guid)client.Id;
    }
}