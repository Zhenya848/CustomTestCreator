namespace CustomTestCreator.Clients.Domain.ValueObjects;

public record TaskSettings
{
    public bool IsRandomTasks { get; set; }
    public bool IsInfiniteMode { get; set; }

    public TaskSettings()
    {
        
    }
    
    public TaskSettings(bool isRandomTasks, bool isInfiniteMode)
    {
        IsRandomTasks = isRandomTasks;
        IsInfiniteMode = isInfiniteMode;
    }
}