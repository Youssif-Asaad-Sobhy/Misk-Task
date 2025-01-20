namespace Misk_Task.Models
{
    public class Orders
    {
        
            public Guid Id { get; set; }
            public DateTime OrderDate { get; set; }
            public OrderStatus Status { get; set; }
            public double TotalAmount { get; set; }
            public List<Products> Products { get; set; } = new();

    }
    public enum OrderStatus
    {
        Pending,
        Completed,
        Cancelled
    }
}
