using System.Data;
using CustomTestCreator.Accounts.Infrastructure.DbContexts;
using CustomTestCreator.Core.Application.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace CustomTestCreator.Accounts.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AccountDbContext _accountDbContext;

    public UnitOfWork(AccountDbContext accountDbContext)
    {
        _accountDbContext = accountDbContext;
    }
    
    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _accountDbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _accountDbContext.SaveChangesAsync(cancellationToken);
    }
}