namespace SemanticTest
{
    public interface IPizzaService
    {


        Task<Menu> GetMenu();
        Task<CartDelta> AddPizzaToCart(
            Guid cartId,
            PizzaSize size,
            List<PizzaToppings> toppings,
            int quantity,
            string specialInstructions);
        Task<RemovePizzaResponse> RemovePizzaFromCart(Guid cartId, int pizzaId);
        Task<Pizza?> GetPizzaFromCart(Guid cartId, int pizzaId);
        Task<Cart> GetCart(Guid cartId);
        Task<CheckoutResponse> Checkout(Guid cartId, Guid paymentId);
    }
}
