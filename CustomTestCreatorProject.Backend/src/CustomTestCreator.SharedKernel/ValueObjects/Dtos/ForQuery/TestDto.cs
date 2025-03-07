namespace CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;

public record TestDto
{
    public Guid Id { get; set; }
    
    public string TestName { get; set; }

    public LimitTimeDto LimitTime { get; set; }
    public bool IsTimeLimited { get; set; }
    
    public IEnumerable<string> Verdicts { get; set; }
}