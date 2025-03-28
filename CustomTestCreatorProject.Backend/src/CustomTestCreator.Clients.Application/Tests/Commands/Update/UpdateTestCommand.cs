namespace CustomTestCreator.Clients.Application.Tests.Commands.Update;

public record UpdateTestCommand(
    Guid TestId,
    Guid ClientId,
    string TestName,
    bool IsPublished,
    int Seconds,
    int Minutes,
    int Hours,
    bool IsTimeLimited,
    IEnumerable<string> VerdictsList);