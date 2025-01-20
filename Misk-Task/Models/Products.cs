namespace Misk_Task.Models
{
    public class Products
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Quantity { get; set; }
        
    }
}
