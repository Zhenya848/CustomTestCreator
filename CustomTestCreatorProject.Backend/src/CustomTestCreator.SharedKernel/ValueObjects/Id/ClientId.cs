namespace CustomTestCreator.SharedKernel.ValueObjects.Id;

public record ClientId
{
    public Guid Value { get; }
    
    public ClientId(Guid value) => Value = value;
    
    public static ClientId AddNewId() => new (Guid.NewGuid());
    
    public static ClientId AddEmptyId() => new (Guid.Empty);
    
    public static ClientId Create(Guid id) => new (id);

    public static implicit operator Guid(ClientId testId)
    {
        ArgumentNullException.ThrowIfNull(testId);
        
        return testId.Value;
    }
}