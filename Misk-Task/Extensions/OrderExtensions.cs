using Misk_Task.Models;

namespace Misk_Task.Extensions
{
    public static class OrderExtensions
    {
        public static double CalculateTotalAmount(this Orders order)
        {
            return order.Products.Sum(p => p.Price * p.Quantity);
        }
    }
}
