using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Application.Repositories;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Clients.Application.Tests.Commands.Delete;

public class DeleteTestHandler : ICommandHandler<DeleteTestCommand, Result<Guid, ErrorList>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTestHandler(
        IClientRepository clientRepository, 
        [FromKeyedServices(Modules.Client)] IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteTestCommand command, 
        CancellationToken cancellationToken = default)
    {
        var clientResult = await _clientRepository
            .GetById(ClientId.Create(command.ClientId), cancellationToken);
        
        if (clientResult.IsFailure)
            return clientResult.Error;
        
        var testResult = clientResult.Value.Tests
            .FirstOrDefault(i => i.Id == command.TestId);
        
        if (testResult == null)
            return (ErrorList)Errors.General.NotFound(command.TestId);

        _clientRepository.DeleteTest(testResult);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return command.TestId;
    }
}