using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Domain.ValueObjects;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos;
using CustomTestCreator.SharedKernel.ValueObjects.Id;

namespace CustomTestCreator.Clients.Domain;

public class Client : SoftDeletableEntity<ClientId>
{
    public string Name { get; private set; }
    public TaskSettings TaskSettings { get; private set; }
    
    private List<Test> _tests = new List<Test>();
    public IReadOnlyList<Test> Tests => _tests;

    private Client(ClientId id) : base(id)
    {
        
    }
    
    public Client(
        ClientId id,
        string name,
        TaskSettings? taskSettings = null,
        IEnumerable<Test>? tests = null) : base(id)
    {
        Name = name;
        TaskSettings = taskSettings ?? new TaskSettings();
        _tests = tests?.ToList() ?? [];
    }

    public override void Delete()
    {
        base.Delete();

        foreach (var test in Tests)
            test.Delete();
    }

    public override void Restore()
    {
        base.Restore();

        foreach (var test in Tests)
            test.Restore();
    }

    public void UpdateInfo(string name, TaskSettings taskSettings)
    {
        Name = name;
        TaskSettings = taskSettings;
    }

    public void AddTest(Test test) =>
        _tests.Add(test);

    public Result<Test, Error> GetTestById(TestId testId)
    {
        var test = _tests.FirstOrDefault(t => t.Id == testId);
        
        if (test == null)
            return Errors.General.NotFound(testId);

        return test;
    }
    
    public UnitResult<Error> UpdateTestInfo(
        Guid testId,
        string testName,
        LimitTime limitTime,
        bool isTimeLimited,
        IEnumerable<string> verdictsList)
    {
        var test = _tests.FirstOrDefault(t => t.Id == testId);
        
        if (test == null)
            return Errors.General.NotFound(testId);
        
        test.UpdateInfo(
            testName, 
            limitTime, 
            isTimeLimited, 
            verdictsList);
        
        return Result.Success<Error>();
    }
}