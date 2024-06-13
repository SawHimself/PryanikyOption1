namespace Option1.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Orders products { get; set; } = null!;
    }
}
