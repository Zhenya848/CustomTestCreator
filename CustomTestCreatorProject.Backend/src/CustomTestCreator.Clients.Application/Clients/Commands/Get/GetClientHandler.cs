using CSharpFunctionalExtensions;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.Core.Application.Repositories;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;
using Microsoft.EntityFrameworkCore;

namespace CustomTestCreator.Clients.Application.Clients.Commands.Get;

public class GetClientHandler : IQueryHandler<Guid, Result<ClientDto, ErrorList>>
{
    private readonly IReadDbContext _readDbContext;

    public GetClientHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<ClientDto, ErrorList>> Handle(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        var clientQuery = _readDbContext.Clients;
        var testQuery = _readDbContext.Tests;
        var tasksQuery = _readDbContext.Tasks;
        
        var client = await clientQuery
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        
        if (client == null)
            return (ErrorList)Errors.General.NotFound(id);
        
        var tests = await testQuery
            .Where(ci => ci.ClientId == id)
            .ToListAsync(cancellationToken);
        
        var testIds = tests.Select(i => i.Id);
        
        var tasks = await tasksQuery
            .Where(task => testIds.Contains(task.TestId))
            .ToListAsync(cancellationToken);

        tests.ForEach(test => test.Tasks = tasks
            .Where(task => task.TestId == test.Id)
            .ToArray());
        
        client.Tests = tests.ToArray();
        
        return client;
    }
}