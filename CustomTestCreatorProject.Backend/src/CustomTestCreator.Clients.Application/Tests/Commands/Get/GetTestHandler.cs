using CSharpFunctionalExtensions;
using CustomTestCreator.Core.Application.Repositories;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;
using Microsoft.EntityFrameworkCore;

namespace CustomTestCreator.Clients.Application.Tests.Commands.Get;

public class GetTestHandler : IQueryHandler<Guid, Result<TestDto, ErrorList>>
{
    private readonly IReadDbContext _readDbContext;

    public GetTestHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<TestDto, ErrorList>> Handle(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        var test = await _readDbContext.Tests
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (test == null)
            return (ErrorList)Errors.General.NotFound(id);

        return test;
    }
}