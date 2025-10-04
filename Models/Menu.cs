namespace SemanticTest{
    // Clase Menu
    public class Menu
    {
        public List<PizzaMenuItem> AvailablePizzas { get; set; } = new();
        public Dictionary<PizzaSize, decimal> SizePrices { get; set; } = new();
        public List<ToppingInfo> AvailableToppings { get; set; } = new();
        public string Currency { get; set; } = "USD";
    }
}