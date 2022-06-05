using Microsoft.EntityFrameworkCore;

namespace EFPerformance;

public class EFDataSourceTracking : IDataSource
{
    public void GetCustomers()
    {
        using var context = new EFContext();
        var customers = context.Set<Customer>()
            .Include(c => c.Group)
                .ThenInclude(g => g.Comments)
            .Include(c => c.Orders)
                .ThenInclude(o => o.Items)
            .Take(Config.TestCount)
            .ToArray();

        foreach (var customer in customers)
        {
            // resolve data
        }
    }
}

public class EFDataSourceNoTracking : IDataSource
{
    public void GetCustomers()
    {
        using var context = new EFContext();
        var customers = context.Set<Customer>()
            .AsNoTracking()
            .Include(c => c.Group)
                .ThenInclude(g => g.Comments)
            .Include(c => c.Orders)
                .ThenInclude(o => o.Items)
            .Take(Config.TestCount)
            .ToArray();

        foreach (var customer in customers)
        {
            // resolve data
        }
    }
}

public class EFRawDataSource : IDataSource
{
    public void GetCustomers()
    {
        using var context = new EFContext();

        var customers = context.Set<RawRecord>()
            .FromSqlRaw(Program.SQL)
            .ToArray();

        foreach (var customer in customers)
        {
            // resolve data
        }
    }
}

public class EFContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<RawRecord> RawRecords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseMySql(Config.ConnectionString, new MySqlServerVersion(new Version(8, 0, 29)))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            // .LogTo(Console.WriteLine);
    }

}
