namespace CustomTestCreator.Clients.Presentation.Clients.Requests;

public record UpdateTestRequest(
    string TestName,
    int Seconds,
    int Minutes,
    int Hours,
    bool IsTimeLimited,
    IEnumerable<string> VerdictsList);