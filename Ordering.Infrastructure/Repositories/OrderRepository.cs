using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entitiies;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository(OrderContext dbContext)
    : RepositoryBase<Order>(dbContext),
        IOrderRepository
{
    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
    {
        var orderList = await _dbContext.Orders.Where(o => o.UserName == userName).ToListAsync();
        return orderList;
    }
}
