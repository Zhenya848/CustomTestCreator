using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Application.Repositories;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.Extensions.DependencyInjection;
using Task = CustomTestCreator.Clients.Domain.Task;

namespace CustomTestCreator.Clients.Application.Tasks.Commands.Create;

public class CreateTasksHandler : ICommandHandler<CreateTasksCommand, Result<IEnumerable<Guid>, ErrorList>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTasksHandler(
        IClientRepository clientRepository, 
        [FromKeyedServices(Modules.Client)]IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<IEnumerable<Guid>, ErrorList>> Handle(
        CreateTasksCommand command, 
        CancellationToken cancellationToken = default)
    {
        var clientResult = await _clientRepository
            .GetById(ClientId.Create(command.ClientId), cancellationToken);
        
        if (clientResult.IsFailure)
            return clientResult.Error;

        var test = clientResult.Value.Tests
            .FirstOrDefault(i => i.Id == command.TestId);

        if (test == null)
            return (ErrorList)Errors.General.NotFound(command.TestId);

        var result = new List<Guid>();
        
        var tasks = command.Tasks
            .Select(t =>
            {
                var id = TaskId.AddNewId();
                result.Add(id);
                
                return new Task(id, t.TaskName, t.TaskMessage, t.RightAnswer);
            });
        
        test.AddTasks(tasks);
        _clientRepository.Save(clientResult.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return Result.Success<IEnumerable<Guid>, ErrorList>(result);
    }
}