namespace CustomTestCreator.SharedKernel;

public class Error
{
    private const string SEPARATOR = "|";
    
    public string Code { get; }
    public string Message { get; }
    public ErrorType ErrorType { get; }
    public string? InvalidField { get; } = null;

    private Error(string code, string message, ErrorType errorType, string? invalidField = null)
    {
        Code = code;
        Message = message;
        ErrorType = errorType;
        InvalidField = invalidField;
    }
    
    public static Error Validation(string code, string message, string? invalidField = null) =>
        new Error(code, message, ErrorType.Validation, invalidField);

    public static Error NotFound(string code, string message) =>
        new Error(code, message, ErrorType.NotFound);

    public static Error Failure(string code, string message) =>
        new Error(code, message, ErrorType.Failure);

    public static Error Conflict(string code, string message) =>
        new Error(code, message, ErrorType.Conflict);
    
    public string Serialize() =>
        string.Join(SEPARATOR, Code, Message, ErrorType);

    public static Error Deserialize(string serializedError)
    {
        string[] parts = serializedError.Split(SEPARATOR);
        
        if (parts.Length != 3 || Enum.TryParse(parts[2], out ErrorType type) == false)
            throw new ArgumentException("Invalid serialized error format!");
        
        return new Error(parts[0], parts[1], type);
    }
}