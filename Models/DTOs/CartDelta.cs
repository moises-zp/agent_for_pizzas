namespace SemanticTest{
    // Clase CartDelta
    public class CartDelta
    {
        public Pizza? AddedPizza { get; set; }
        public Cart? UpdatedCart { get; set; }
        public decimal PreviousTotal { get; set; }
        public decimal NewTotal { get; set; }
        public string? Message { get; set; }
    }
}