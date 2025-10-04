namespace SemanticTest
{
    public class PizzaItem
    {
        public int Id { get; set; }
        public PizzaSize Size { get; set; }
        public List<PizzaToppings> Toppings { get; set; } = new();
        public int Quantity { get; set; }
        public string? SpecialInstructions { get; set; }
        public decimal Price { get; set; }
    }
}