namespace CustomTestCreator.Clients.Presentation.Clients.Requests;

public record UpdateTestRequest(
    string TestName,
    bool IsPublished,
    int Seconds,
    int Minutes,
    int Hours,
    bool IsTimeLimited,
    IEnumerable<string> VerdictsList);