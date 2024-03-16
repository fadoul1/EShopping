using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ordering.Core.Common;
using Ordering.Core.Entitiies;

namespace Ordering.Infrastructure.Data;

public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = "rahul"; //TODO: This will be replaced Identity Server
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    entry.Entity.LastModifiedBy = "rahul"; //TODO: This will be replaced Identity Server
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
