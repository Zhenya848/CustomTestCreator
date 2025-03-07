namespace CustomTestCreator.SharedKernel.ValueObjects.Dtos
{
    public record UploadFileDto(string FileName, string ContentType, Stream Stream);
}
