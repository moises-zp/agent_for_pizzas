namespace SemanticTest{
    public class PizzaMenuItem
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<PizzaToppings> DefaultToppings { get; set; } = new();
        public bool IsSpecialty { get; set; }
    }
}