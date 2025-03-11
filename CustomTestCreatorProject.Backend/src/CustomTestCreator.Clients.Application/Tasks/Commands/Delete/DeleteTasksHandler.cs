using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Application.Repositories;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.Core.Application.Messaging;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.Extensions.DependencyInjection;
using FileInfo = CustomTestCreator.SharedKernel.ValueObjects.FileInfo;

namespace CustomTestCreator.Clients.Application.Tasks.Commands.Delete;

public class DeleteTasksHandler : ICommandHandler<DeleteTasksCommand, Result<IEnumerable<Guid>, ErrorList>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientRepository _clientRepository;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public DeleteTasksHandler(
        [FromKeyedServices(Modules.Client)]IUnitOfWork unitOfWork, 
        IClientRepository clientRepository,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _unitOfWork = unitOfWork;
        _clientRepository = clientRepository;
        _messageQueue = messageQueue;
    }
    
    public async Task<Result<IEnumerable<Guid>, ErrorList>> Handle(
        DeleteTasksCommand command, 
        CancellationToken cancellationToken = default)
    {
        var clientResult = await _clientRepository
            .GetById(ClientId.Create(command.ClientId), cancellationToken);

        if (clientResult.IsFailure)
            return clientResult.Error;
        
        var testResult = clientResult.Value
            .GetTestById(TestId.Create(command.TestId));
        
        if (testResult.IsFailure)
            return (ErrorList)testResult.Error;
        
        var tasks = testResult.Value
            .GetTasksByIds(command.TasIds);

        var files = tasks
            .Select(t => new FileInfo("photos", t.ImagePath));
        
        await _messageQueue.WriteAsync(files, cancellationToken);
        _clientRepository.DeleteTasks(tasks);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return Result.Success<IEnumerable<Guid>, ErrorList>(tasks.Select(i => i.Id.Value));
    }
}