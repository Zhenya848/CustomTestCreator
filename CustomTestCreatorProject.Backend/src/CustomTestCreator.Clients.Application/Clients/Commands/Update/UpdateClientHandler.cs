using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Application.Repositories;
using CustomTestCreator.Clients.Domain.ValueObjects;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Clients.Application.Clients.Commands.Update;

public class UpdateClientHandler : ICommandHandler<UpdateClientCommand, Result<Guid, ErrorList>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientRepository _clientRepository;

    public UpdateClientHandler(
        [FromKeyedServices(Modules.Client)]IUnitOfWork unitOfWork, 
        IClientRepository clientRepository)
    {
        _unitOfWork = unitOfWork;
        _clientRepository = clientRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateClientCommand command, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return (ErrorList)Errors.General.ValueIsRequired(command.Name);
        
        var clientResult = await _clientRepository
            .GetById(ClientId.Create(command.ClientId), cancellationToken);

        if (clientResult.IsFailure)
            return clientResult.Error;
        
        var taskSettings = new TaskSettings(command.IsRandomTasks, command.IsInfiniteMode);
        clientResult.Value.UpdateInfo(command.Name, taskSettings);
        
        _clientRepository.Save(clientResult.Value);
        await _unitOfWork.SaveChanges(cancellationToken);

        return (Guid)clientResult.Value.Id;
    }
}