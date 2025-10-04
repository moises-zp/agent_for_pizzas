using System.Text.Json.Serialization;

namespace SemanticTest
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PizzaSize
    {
        Small,
        Medium,
        Large
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PizzaToppings
    {
        Cheese,
        Pepperoni,
        Mushrooms
    }
    // Clase Pizza
    public class Pizza
    {
        public int Id { get; set; }
        public PizzaSize Size { get; set; }
        public List<PizzaToppings> Toppings { get; set; } = new();
        public int Quantity { get; set; }
        public string? SpecialInstructions { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime AddedAt { get; set; }

        public Pizza()
        {
            AddedAt = DateTime.UtcNow;
        }
    }

}