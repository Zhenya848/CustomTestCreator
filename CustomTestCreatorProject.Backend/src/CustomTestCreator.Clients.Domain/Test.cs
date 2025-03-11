using CustomTestCreator.Clients.Domain.ValueObjects;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects.Id;

namespace CustomTestCreator.Clients.Domain;

public class Test : SoftDeletableEntity<TestId>
{
    public string TestName { get; private set; }
    
    public LimitTime LimitTime { get; private set; }
    public bool IsTimeLimited { get; private set; }
    
    private List<Task> _tasks  = new List<Task>();
    public IReadOnlyList<Task> Tasks => _tasks;
    
    public List<string> VerdictsList;

    private Test(TestId id) : base(id)
    {
        
    }
    
    public Test(
        TestId id, 
        string testName, 
        LimitTime limitTime, 
        bool isTimeLimited, 
        IEnumerable<string>? verdictsList = null,
        IEnumerable<Task>? tasks = null) : base(id)
    {
        TestName = testName;
        LimitTime = limitTime;
        IsTimeLimited = isTimeLimited;
        _tasks = tasks?.ToList() ?? [];
        VerdictsList = verdictsList?.ToList() ?? [];
    }

    public void AddTasks(IEnumerable<Task> tasks) =>
        _tasks.AddRange(tasks);

    public List<Task> GetTasksByIds(IEnumerable<Guid> TaskIds)
    {
        var result = new List<Task>();

        foreach (var taskId in TaskIds)
        {
            var task = _tasks.FirstOrDefault(i => i.Id == taskId);
            
            if (task != null)
                result.Add(task);
        }
        
        return result;
    }
    
    internal void UpdateInfo(
        string testName,
        LimitTime limitTime,
        bool isTimeLimited,
        IEnumerable<string> verdictsList)
    {
        TestName = testName;
        LimitTime = limitTime;
        IsTimeLimited = isTimeLimited;
        VerdictsList = verdictsList.ToList();
    }

    public override void Delete()
    {
        base.Delete();

        foreach (var task in Tasks)
            task.Delete();
    }

    public override void Restore()
    {
        base.Restore();

        foreach (var task in Tasks)
            task.Restore();
    }
}