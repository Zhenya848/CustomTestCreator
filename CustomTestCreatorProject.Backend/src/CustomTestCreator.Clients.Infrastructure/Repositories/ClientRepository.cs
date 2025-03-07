using CSharpFunctionalExtensions;
using CustomTestCreator.Clients.Application.Repositories;
using CustomTestCreator.Clients.Domain;
using CustomTestCreator.Clients.Infrastructure.DbContexts;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CustomTestCreator.Clients.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ClientDbContext _context;
    private readonly ILogger<ClientRepository> _logger;

    public ClientRepository(
        ClientDbContext context,
        ILogger<ClientRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public Guid Add(Client client)
    {
        _context.Clients.Add(client);
        _logger.LogInformation("Created client {client} with id {id}", client.Name, client.Id.Value);
        
        return client.Id;
    }

    public Guid Save(Client client)
    {
        _context.Clients.Attach(client);
        _logger.LogInformation("Updated client {client} with id {id}", client.Name, client.Id.Value);
        
        return client.Id;
    }

    public async Task<Result<Client, ErrorList>> GetById(
        ClientId id,
        CancellationToken cancellationToken)
    {
        var clients = _context.Clients
            .Include(t => t.Tests)
            .ThenInclude(t => t.Tasks);
        
        var clientResult = await clients
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (clientResult == null)
            return (ErrorList)Errors.General.NotFound(id.Value);

        return clientResult;
    }

    public Guid DeleteTest(Test test)
    {
        _context.Tests.Remove(test);
        _logger.LogInformation("Deleted test {test} with id {id}", test.TestName, test.Id.Value);
        
        return test.Id;
    }
}