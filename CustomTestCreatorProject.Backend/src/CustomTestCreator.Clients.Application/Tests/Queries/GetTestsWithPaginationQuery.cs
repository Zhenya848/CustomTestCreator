namespace CustomTestCreator.Clients.Application.Tests.Queries;

public record GetTestsWithPaginationQuery(
    int Page,
    int PageSize);