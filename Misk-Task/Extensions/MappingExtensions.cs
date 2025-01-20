using Misk_Task.Models.DTOs;
using Misk_Task.Models;

namespace Misk_Task.Extensions
{
    public static class MappingExtensions
    {
        public static OrderDto ToDto(this Orders order)
        {
            return new OrderDto(
                order.Id,
                order.OrderDate,
                order.Status,
                order.TotalAmount,
                order.Products.Select(p => p.ToDto()).ToList()
            );
        }

        public static ProductDto ToDto(this Products product)
        {
            return new ProductDto(
                product.Id,
                product.Name,
                product.Price,
                product.Quantity
            );
        }

        public static Products ToEntity(this CreateProductDto dto)
        {
            return new Products
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity
            };
        }

        public static Orders ToEntity(this CreateOrderDto dto)
        {
            return new Orders
            {
                Products = dto.Products.Select(p => p.ToEntity()).ToList()
            };
        }
    }
}
