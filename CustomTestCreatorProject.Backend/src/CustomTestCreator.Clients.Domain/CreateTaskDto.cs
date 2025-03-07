namespace CustomTestCreator.Clients.Domain;

public record CreateTaskDto(
    string TaskName,
    string TaskMessage,
    string RightAnswer);