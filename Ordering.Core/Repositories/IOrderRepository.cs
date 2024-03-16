using Ordering.Core.Entitiies;

namespace Ordering.Core.Repositories;

public interface IOrderRepository : IAsyncRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
}
