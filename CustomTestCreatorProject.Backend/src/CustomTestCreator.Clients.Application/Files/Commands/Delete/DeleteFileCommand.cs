namespace CustomTestCreator.Clients.Application.Files.Commands.Delete;

public record DeleteFileCommand(string BucketName, string ObjectName);