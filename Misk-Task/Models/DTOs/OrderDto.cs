namespace Misk_Task.Models.DTOs
{
       
    public record OrderDto(
        Guid Id,
        DateTime OrderDate,
        OrderStatus Status,
        double TotalAmount,
        ICollection<ProductDto> Products
    );

    public record CreateOrderDto(
        ICollection<CreateProductDto> Products
    );
}
