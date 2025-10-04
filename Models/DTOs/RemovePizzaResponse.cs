namespace SemanticTest{
    // Clase RemovePizzaResponse
    public class RemovePizzaResponse
    {
        public bool Success { get; set; }
        public int RemovedPizzaId { get; set; }
        public Cart? UpdatedCart { get; set; }
        public decimal PreviousTotal { get; set; }
        public decimal NewTotal { get; set; }
        public string? Message { get; set; }
    }
}