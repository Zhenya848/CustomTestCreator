using CustomTestCreator.SharedKernel.ValueObjects.Dtos;

namespace CustomTestCreator.Clients.Application.Tasks.Commands.UploadPhotos;

public record UploadFilesToTasksCommand(
    Guid ClientId,
    Guid TestId,
    IEnumerable<Guid> TaskIds,
    IEnumerable<UploadFileDto> Files);