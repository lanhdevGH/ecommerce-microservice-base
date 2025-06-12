using Common.Logging;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Ordering.Infrastructure.Persistence;

public class OrderDBContextSeed
{
    private readonly ILogger _logger = Log.ForContext<OrderDBContextSeed>();
    private readonly OrderDBContext _context;

    public OrderDBContextSeed(ILogger logger, OrderDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Err(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Err(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_context.Orders.Any())
        {
            await _context.Orders.AddRangeAsync(
                new Order
                {
                    UserName = "customer1",
                    FirstName = "customer1",
                    LastName = "customer",
                    EmailAddress = "customer1@local.com",
                    ShippingAddress = "Wollongong",
                    InvoiceAddress = "Australia",
                    TotalPrice = 250
                });
        }
    }
}
