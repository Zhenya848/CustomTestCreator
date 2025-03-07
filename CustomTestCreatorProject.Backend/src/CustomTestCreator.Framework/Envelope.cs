using CustomTestCreator.SharedKernel;

namespace CustomTestCreator.Framework;

public class Envelope
{
    public object? Result { get; }
    public ErrorList? ResponseErrors { get; }
    
    public DateTime? Time { get; }

    private Envelope(object? result, ErrorList? responseErrors)
    {
        Result = result;
        ResponseErrors = responseErrors;
        
        Time = DateTime.Now;
    }

    public static Envelope Error(ErrorList errors) =>
        new (null, errors);
    
    public static Envelope Ok(object? result) =>
        new (result, null);
}