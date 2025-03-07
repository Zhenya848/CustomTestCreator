using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Application.Repositories;
using CustomTestCreator.Clients.Domain;
using CustomTestCreator.Clients.Domain.ValueObjects;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Clients.Application.Tests.Commands.Create;

public class CreateTestHandler : ICommandHandler<CreateTestCommand, Result<Guid, ErrorList>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTestHandler(
        [FromKeyedServices(Modules.Client)] IUnitOfWork unitOfWork,
        IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateTestCommand command, 
        CancellationToken cancellationToken = default)
    {
        var clientResult = await _clientRepository
            .GetById(ClientId.Create(command.ClientId), cancellationToken);
        
        if (clientResult.IsFailure)
            return clientResult.Error;
        
        if (string.IsNullOrWhiteSpace(command.TestName))
            return (ErrorList)Errors.General.ValueIsRequired("Test name");
        
        var limitTimeResult = LimitTime.Create(command.Seconds, command.Minutes, command.Hours);
        
        if (limitTimeResult.IsFailure)
            return (ErrorList)limitTimeResult.Error;
        
        var test = new Test(
            TestId.AddNewId(), 
            command.TestName, 
            limitTimeResult.Value, 
            command.IsTimeLimited,
            command.VerdictsList);
        
        clientResult.Value.AddTest(test);
        _clientRepository.Save(clientResult.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return (Guid)test.Id;
    }
}