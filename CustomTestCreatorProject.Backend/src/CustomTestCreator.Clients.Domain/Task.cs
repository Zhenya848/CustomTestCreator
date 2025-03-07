using CustomTestCreator.Clients.Domain.ValueObjects;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects.Id;

namespace CustomTestCreator.Clients.Domain;

public class Task : SoftDeletableEntity<TaskId>
{
    public string TaskName { get; private set; }
    public string TaskMessage { get; private set; }
    public string RightAnswer { get; private set; }
    
    public string ImagePath { get; private set; }

    protected Task(TaskId id) : base(id)
    {
        
    }
    
    public Task(
        TaskId id, 
        string taskName, 
        string taskMessage, 
        string rightAnswer,
        string? imagePath = null) : base(id)
    {
        TaskName = taskName;
        TaskMessage = taskMessage;
        RightAnswer = rightAnswer;
        ImagePath = imagePath ?? string.Empty;
    }

    public void UpdateImagePath(string imagePath) =>
        ImagePath = imagePath;
    
    public override void Delete()
    {
        base.Delete();
    }

    public override void Restore()
    {
        base.Restore();
    }
}