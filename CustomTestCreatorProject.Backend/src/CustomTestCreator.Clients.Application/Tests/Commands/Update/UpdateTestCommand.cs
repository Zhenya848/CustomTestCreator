namespace CustomTestCreator.Clients.Application.Tests.Commands.Update;

public record UpdateTestCommand(
    Guid TestId,
    Guid ClientId,
    string TestName,
    int Seconds,
    int Minutes,
    int Hours,
    bool IsTimeLimited,
    IEnumerable<string> VerdictsList);