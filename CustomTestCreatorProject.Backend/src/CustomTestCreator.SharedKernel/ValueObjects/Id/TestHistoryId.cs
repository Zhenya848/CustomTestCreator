namespace CustomTestCreator.SharedKernel.ValueObjects.Id;

public class TestHistoryId
{
    public Guid Value { get; }
    
    public TestHistoryId(Guid value) => Value = value;
    
    public static TestHistoryId AddNewId() => new (Guid.NewGuid());
    
    public static TestHistoryId AddEmptyId() => new (Guid.Empty);
    
    public static TestHistoryId Create(Guid id) => new (id);

    public static implicit operator Guid(TestHistoryId testId)
    {
        ArgumentNullException.ThrowIfNull(testId);
        
        return testId.Value;
    }
}