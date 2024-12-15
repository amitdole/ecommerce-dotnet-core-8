using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }
        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            {
                new Order()
                {
                    UserName = "testshopper",
                    FirstName = "Test",
                    LastName = "Shopper",
                    EmailAddress = "testshopper@eCommerce.net",
                    AddressLine = "Test Address Line",
                    Country = "US",
                    State = "TX",
                    ZipCode = "87977",

                    CardName = "Test CardName",
                    CardNumber = "1234567890123456",
                    Expiration = "12/23",
                    CVV = "123",
                    PaymentMethod = 1,
                    TotalPrice = 750,
                    LastModifiedDate = DateTime.Now,
                    LastModifiedBy = "System"
                }
            };
        }
    }
}


