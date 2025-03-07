using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Application.Files.Commands.Create;
using CustomTestCreator.Clients.Application.Files.Commands.Delete;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects;

namespace CustomTestCreator.Clients.Application.Providers;

public interface IFileProvider
{
    public Task<Result<IReadOnlyList<string>, Error>> UploadFiles(
        CreateFilesCommand command,
        CancellationToken cancellationToken = default);
    
    public Task<Result<string, Error>> DeleteFile(
        DeleteFileCommand command,
        CancellationToken cancellationToken = default);
    
    public Task<Result<IReadOnlyList<FileData>, Error>> GetFiles(CancellationToken cancellationToken = default);
}