using Contracts.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public class UnitOfWork<TContext>(TContext context) : IUnitOfWork<TContext> where TContext : DbContext
{
    private readonly TContext _context = context; // Fix for CS9113: Assign 'context' to a private readonly field  

    public Task<int> CommitAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this); // Fix for CA1816    
    }
}
