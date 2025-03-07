namespace CustomTestCreator.Clients.Application.Tests.Commands.Delete;

public record DeleteTestCommand(
    Guid ClientId,
    Guid TestId);