using CustomTestCreator.Core.Application;
using CustomTestCreator.Core.Application.Extensions;
using CustomTestCreator.Core.Application.Repositories;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;
using Microsoft.EntityFrameworkCore;

namespace CustomTestCreator.Clients.Application.Tests.Queries;

public class GetTestsWithPaginationHandler : IQueryHandler<GetTestsWithPaginationQuery, PagedList<TestDto>>
{
    private readonly IReadDbContext _readDbContext;

    public GetTestsWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<TestDto>> Handle(
        GetTestsWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var testsQuery = _readDbContext.Tests
            .Where(ip => ip.IsPublished)
            .GetItemsWithPagination(query.Page, query.PageSize);

        var tests = await testsQuery.ToListAsync(cancellationToken);
        var testIds = tests.Select(i => i.Id);
        
        var tasks = await _readDbContext.Tasks
            .Where(t => testIds.Contains(t.TestId))
            .ToListAsync(cancellationToken);
        
        tests.ForEach(test => test.Tasks = tasks
            .Where(t => t.TestId == test.Id)
            .ToArray());

        var result = new PagedList<TestDto>()
        {
            Items = tests,
            TotalCount = tests.Count,
            Page = query.Page,
            PageSize = query.PageSize,
        };
        
        return result;
    }
}