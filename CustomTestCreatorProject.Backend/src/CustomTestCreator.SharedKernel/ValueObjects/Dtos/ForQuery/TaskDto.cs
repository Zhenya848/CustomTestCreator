namespace CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;

public record TaskDto
{
    public string TaskName { get; set; }
    public string TaskMessage { get; set; }
    public string RightAnswer { get; set; }

    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    public string ImagePath { get; set; }
}