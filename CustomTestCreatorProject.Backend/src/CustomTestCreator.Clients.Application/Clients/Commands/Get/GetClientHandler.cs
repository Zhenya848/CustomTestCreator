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
        var clientResult = await clientQuery
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        
        var tests = await _readDbContext.Tests.ToListAsync(cancellationToken);
        var tasks = await _readDbContext.Tasks.ToListAsync(cancellationToken);

        if (clientResult == null)
            return (ErrorList)Errors.General.NotFound(id);
        
        return clientResult;
    }
}