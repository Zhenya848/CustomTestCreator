namespace CustomTestCreator.SharedKernel.ValueObjects.Id;

public record TestId
{
    public Guid Value { get; }
    
    public TestId(Guid value) => Value = value;
    
    public static TestId AddNewId() => new (Guid.NewGuid());
    
    public static TestId AddEmptyId() => new (Guid.Empty);
    
    public static TestId Create(Guid id) => new (id);

    public static implicit operator Guid(TestId testId)
    {
        ArgumentNullException.ThrowIfNull(testId);
        
        return testId.Value;
    }
}