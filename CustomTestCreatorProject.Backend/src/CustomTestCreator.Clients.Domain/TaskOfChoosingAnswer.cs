using CustomTestCreator.SharedKernel.ValueObjects.Id;

namespace CustomTestCreator.Clients.Domain;

public class TaskOfChoosingAnswer : Task
{
    public List<string> AnswersList { get; }
    
    private TaskOfChoosingAnswer(TaskId id) : base(id)
    {
        
    }
    
    public TaskOfChoosingAnswer(
        TaskId id,
        string taskName,
        string taskMessage,
        string rightAnswer,
        IEnumerable<string>? answersList = null) : base(id, taskName, taskMessage, rightAnswer)
    {
        AnswersList = answersList?.ToList() ?? [];
    }
}