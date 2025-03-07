using CustomTestCreator.SharedKernel.ValueObjects;

namespace CustomTestCreator.Clients.Application.Files.Commands.Create;

public record CreateFilesCommand(
    IEnumerable<FileData> Files, 
    string BucketName);