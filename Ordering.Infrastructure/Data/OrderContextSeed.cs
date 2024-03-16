using Microsoft.Extensions.Logging;
using Ordering.Core.Entitiies;

namespace Ordering.Infrastructure.Data;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation(
                "Ordering Database: {database} seeded.",
                typeof(OrderContext).Name
            );
        }
    }

    private static IEnumerable<Order> GetOrders()
    {
        return
        [
            new()
            {
                UserName = "rahul",
                FirstName = "Rahul",
                LastName = "Sahay",
                EmailAddress = "rahulsahay@eshop.net",
                AddressLine = "Bangalore",
                Country = "India",
                TotalPrice = 750,
                State = "KA",
                ZipCode = "560001",

                CardName = "Visa",
                CardNumber = "1234567890123456",
                CreatedBy = "Rahul",
                Expiration = "12/25",
                Cvv = "123",
                PaymentMethod = 1,
                LastModifiedBy = "Rahul",
                LastModifiedDate = DateTime.Now,
            }
        ];
    }
}
