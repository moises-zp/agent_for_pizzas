namespace SemanticTest{
    // Clase CheckoutResponse
    public class CheckoutResponse
    {
        public bool Success { get; set; }
        public Guid OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Subtotal { get; set; }
        public string? OrderStatus { get; set; }
        public DateTime OrderPlacedAt { get; set; }
        public int EstimatedDeliveryMinutes { get; set; }
        public string? ConfirmationMessage { get; set; }
        public List<Pizza> OrderedPizzas { get; set; } = new();
    }
}