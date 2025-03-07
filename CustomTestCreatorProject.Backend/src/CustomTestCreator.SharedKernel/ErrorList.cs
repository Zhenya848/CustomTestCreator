using System.Collections;

namespace CustomTestCreator.SharedKernel;

public class ErrorList : IEnumerable<Error>
{
    private readonly List<Error> Value;
    
    private ErrorList(IEnumerable<Error> errors) =>
        Value = [.. errors];
    
    public IEnumerator<Error> GetEnumerator()
    {
        return Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static implicit operator ErrorList(List<Error> errors) =>
        new(errors);
    
    public static implicit operator ErrorList(Error error) =>
        new ([error]);
}