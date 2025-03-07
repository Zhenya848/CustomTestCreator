using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Application.Files.Commands.Create;
using CustomTestCreator.Clients.Application.Providers;
using CustomTestCreator.Clients.Application.Repositories;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.Core.Application.Validation;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Application.Messaging;
using PetProject.Core.ValueObjects;
using PetProject.Volunteers.Application.Validators;
using FileInfo = CustomTestCreator.SharedKernel.ValueObjects.FileInfo;

namespace CustomTestCreator.Clients.Application.Tasks.Commands.UploadPhotos;

public class UploadFilesToTasksHandler : ICommandHandler<UploadFilesToTasksCommand, Result<IEnumerable<string>, ErrorList>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IClientRepository _clientRepository;
    private readonly IFileProvider _fileProvider;
    
    private readonly IValidator<UploadFilesToTasksCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public UploadFilesToTasksHandler(
        [FromKeyedServices(Modules.Client)] IUnitOfWork unitOfWork,
        IClientRepository clientRepository,
        IFileProvider fileProvider,
        IValidator<UploadFilesToTasksCommand> validator,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _unitOfWork = unitOfWork;
        _clientRepository = clientRepository;
        _fileProvider = fileProvider;
        _validator = validator;
        _messageQueue = messageQueue;
    }
    
    public async Task<Result<IEnumerable<string>, ErrorList>> Handle(
        UploadFilesToTasksCommand command, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator
            .ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ValidationErrorResponse();
        
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var clientResult = await _clientRepository
                .GetById(ClientId.Create(command.ClientId), cancellationToken);

            if (clientResult.IsFailure)
                return clientResult.Error;

            var testResult = clientResult.Value.GetTestById(TestId.Create(command.TestId));

            if (testResult.IsFailure)
                return (ErrorList)testResult.Error;

            var tasks = testResult.Value.GetTasksByIds(command.TaskIds);

            if (tasks.Count == 0)
                return (ErrorList)Errors.General.ValueIsInvalid("Tasks not found");
            
            if (tasks.Count != command.Files.Count())
                return (ErrorList)Errors.General.ValueIsInvalid("Number of files does not match");

            List<FileData> files = new List<FileData>();

            for (int i = 0; i < tasks.Count; i++)
            {
                var file = command.Files.ElementAt(i);
                var pathResult = FilePath.Create(file.FileName);

                if (pathResult.IsFailure)
                    return (ErrorList)pathResult.Error;

                tasks[i].UpdateImagePath(pathResult.Value.FullPath);

                var fileData = new FileData(pathResult.Value.FullPath, file.Stream);
                files.Add(fileData);
            }

            await _unitOfWork.SaveChanges(cancellationToken);

            var createFilesCommand = new CreateFilesCommand(files, "photos");
            var uploadResult = await _fileProvider.UploadFiles(createFilesCommand, cancellationToken);

            if (uploadResult.IsFailure)
            {
                List<FileInfo> filesInfo =
                    files.Select(f => new FileInfo("photos", f.FilePath)).ToList();
                
                await _messageQueue.WriteAsync(filesInfo, cancellationToken);
                
                return (ErrorList)uploadResult.Error;
            }

            transaction.Commit();

            var result = files.Select(p => p.FilePath);

            return Result.Success<IEnumerable<string>, ErrorList>(result);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            
            return (ErrorList)Error.Failure("Can not to upload files to pet. " + ex.Message, "upload.files.failure");
        }
    }
}