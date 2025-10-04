namespace SemanticTest
{
    // Modelos de datos
    public class PizzaMenu
    {
        public List<MenuItem> Items { get; set; } = new();
        public Dictionary<PizzaSize, decimal> Prices { get; set; } = new();
        public List<string> AvailableToppings { get; set; } = new();
    }

}
