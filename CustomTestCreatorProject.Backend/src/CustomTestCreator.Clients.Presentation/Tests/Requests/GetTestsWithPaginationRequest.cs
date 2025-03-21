namespace CustomTestCreator.Clients.Presentation.Tests.Requests;

public record GetTestsWithPaginationRequest(
    int Page,
    int PageSize);