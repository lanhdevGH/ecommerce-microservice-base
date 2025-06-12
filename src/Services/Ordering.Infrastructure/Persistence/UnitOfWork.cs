using Contracts.Common.Interfaces;

namespace Ordering.Infrastructure.Persistence;

internal class UnitOfWork(OrderDBContext context) : IUnitOfWork
{
    private readonly OrderDBContext _context = context;

    public void Dispose() => _context.Dispose();

    public Task<int> CommitAsync() => _context.SaveChangesAsync();
}
