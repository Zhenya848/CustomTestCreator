namespace CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;

public record TestHistoryDto
{
    public Guid Id { get; set; }
    
    public Guid CreatorId { get; set; }
    public Guid TestId { get; set; }
    
    public int CountOfAllAnswers { get; set; }
    public int CountOfRightAnswers { get; set; }
    
    public DateTime PassageTime { get; set; }
}