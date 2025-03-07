using System.Data;
using CustomTestCreator.Clients.Infrastructure.DbContexts;
using CustomTestCreator.Core.Application.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace CustomTestCreator.Clients.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ClientDbContext _context;

    public UnitOfWork(ClientDbContext context)
    {
        _context = context;
    }

    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}