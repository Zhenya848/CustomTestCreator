using CSharpFunctionalExtensions;
using CustomTestCreator.SharedKernel;

namespace CustomTestCreator.Clients.Domain.ValueObjects;

public record LimitTime
{
    public int Seconds { get; }
    public int Minutes { get; }
    public int Hours { get; }

    private LimitTime(int seconds, int minutes, int hours)
    {
        Seconds = seconds;
        Minutes = minutes;
        Hours = hours;
    }
    
    public static Result<LimitTime, Error> Create(int seconds, int minutes, int hours)
    {
        long allSeconds = seconds + minutes * 60 + hours * 3600;
        
        if (allSeconds < 10)
            return Errors.General.ValueIsInvalid("Limit time must be at least 10.");
        
        int hoursResult = (int)(allSeconds / 3600);
        int minutesResult = (int)(allSeconds % 3600 / 60);
        int secondsResult = (int)(allSeconds % 3600 % 60);
        
        return new LimitTime(secondsResult, minutesResult, hoursResult);
    }
}