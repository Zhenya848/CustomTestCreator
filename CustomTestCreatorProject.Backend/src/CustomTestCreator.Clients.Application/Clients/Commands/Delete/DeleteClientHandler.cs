using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Application.Repositories;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Clients.Application.Clients.Commands.Delete;

public class DeleteClientHandler : ICommandHandler<Guid, Result<Guid, ErrorList>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClientHandler(
        IClientRepository clientRepository, 
        [FromKeyedServices(Modules.Client)]IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        var clientResult = await _clientRepository
            .GetById(ClientId.Create(id), cancellationToken);
        
        if (clientResult.IsFailure)
            return clientResult.Error;
        
        clientResult.Value.Delete();
        await _unitOfWork.SaveChanges(cancellationToken);

        return (Guid)clientResult.Value.Id;
    }
}