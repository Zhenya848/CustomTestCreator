using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Application.Repositories;
using CustomTestCreator.Clients.Domain;
using CustomTestCreator.Clients.Domain.ValueObjects;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Clients.Application.Tests.Commands.Update;

public class UpdateTestHandler : ICommandHandler<UpdateTestCommand, Result<Guid, ErrorList>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTestHandler(
        [FromKeyedServices(Modules.Client)] IUnitOfWork unitOfWork,
        IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateTestCommand command, 
        CancellationToken cancellationToken = default)
    {
        var clientResult = await _clientRepository
            .GetById(ClientId.Create(command.ClientId), cancellationToken);
        
        if (clientResult.IsFailure)
            return clientResult.Error;
        
        if (string.IsNullOrWhiteSpace(command.TestName))
            return (ErrorList)Errors.General.ValueIsRequired(command.TestName);
        
        var limitTimeResult = LimitTime.Create(command.Seconds, command.Minutes, command.Hours);
        
        if (limitTimeResult.IsFailure)
            return (ErrorList)limitTimeResult.Error;
        
        var updateTestResult = clientResult.Value.UpdateTestInfo(
            command.TestId,
            command.TestName, 
            command.IsPublished,
            limitTimeResult.Value, 
            command.IsTimeLimited, 
            command.VerdictsList);
        
        if (updateTestResult.IsFailure)
            return (ErrorList)updateTestResult.Error;

        _clientRepository.Save(clientResult.Value);
        await _unitOfWork.SaveChanges(cancellationToken);

        return command.TestId;
    }
}