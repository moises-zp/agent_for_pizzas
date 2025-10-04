namespace SemanticTest{
    public class Cart
    {
        public Guid CartId { get; set; }
        public List<Pizza> Items { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Cart()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}