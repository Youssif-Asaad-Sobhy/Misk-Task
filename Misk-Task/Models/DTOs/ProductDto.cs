namespace Misk_Task.Models.DTOs
{
    
    public record ProductDto(
        Guid Id,
        string Name,
        double Price,
        int Quantity
    );

    public record CreateProductDto(
        string Name,
        double Price,
        int Quantity
    );
}
