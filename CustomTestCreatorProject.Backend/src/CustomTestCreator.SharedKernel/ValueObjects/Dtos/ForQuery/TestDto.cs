using System.ComponentModel.DataAnnotations.Schema;

namespace CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;

public record TestDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    
    public string TestName { get; set; }
    public bool IsPublished { get; set; }

    public LimitTimeDto LimitTime { get; set; }
    public bool IsTimeLimited { get; set; }
    
    public IEnumerable<string> Verdicts { get; set; }
    
    [NotMapped]
    public TaskDto[] Tasks { get; set; }
}