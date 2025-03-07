namespace CustomTestCreator.Clients.Application.Tests.Commands.Create;

public record CreateTestCommand(
    Guid ClientId,
    string TestName,
    int Seconds,
    int Minutes,
    int Hours,
    bool IsTimeLimited,
    IEnumerable<string> VerdictsList);