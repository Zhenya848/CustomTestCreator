using System.Data;

namespace CustomTestCreator.Core.Application.Abstractions;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default);
    
    Task SaveChanges(CancellationToken cancellationToken = default);
}