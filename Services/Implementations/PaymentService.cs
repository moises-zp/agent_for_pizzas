namespace SemanticTest{
public class PizzaService : IPizzaService
    {
        // Simulamos un almacenamiento en memoria para los carritos
        private static readonly Dictionary<Guid, Cart> _carts = new();
        private static int _nextPizzaId = 1;

        public async Task<Menu> GetMenu()
        {
            return await Task.FromResult(
                 new Menu
                 {
                     AvailablePizzas = new List<PizzaMenuItem> {
                    new () {
                        Name = "Margherita",
                        Description = "Classic tomato and mozzarella",
                        DefaultToppings = new List<PizzaToppings> { PizzaToppings.Cheese },
                        IsSpecialty = true
                    },
                    new () {
                        Name = "Pepperoni",
                        Description = "Pepperoni and cheese",
                        DefaultToppings = new List<PizzaToppings> { PizzaToppings.Cheese, PizzaToppings.Pepperoni },
                        IsSpecialty = true
                    },
                    new () {
                        Name = "Vegetarian",
                        Description = "Mushrooms and cheese",
                        DefaultToppings = new List<PizzaToppings> { PizzaToppings.Cheese, PizzaToppings.Mushrooms },
                        IsSpecialty = true
                    }
                },
                     SizePrices = new Dictionary<PizzaSize, decimal> {
                    { PizzaSize.Small, 8.99m },
                    { PizzaSize.Medium, 12.99m },
                    { PizzaSize.Large, 15.99m }
                },
                     AvailableToppings = new List<ToppingInfo> {
                    new () { Topping = PizzaToppings.Cheese, Name = "Cheese", AdditionalPrice = 0m },
                    new () { Topping = PizzaToppings.Pepperoni, Name = "Pepperoni", AdditionalPrice = 1.50m },
                    new () { Topping = PizzaToppings.Mushrooms, Name = "Mushrooms", AdditionalPrice = 1.00m }
                },
                     Currency = "USD"
                 }
            );
        }

        public async Task<CartDelta> AddPizzaToCart(Guid cartId, PizzaSize size, List<PizzaToppings> toppings, int quantity, string specialInstructions)
        {
            // Obtener o crear el carrito
            if (!_carts.ContainsKey(cartId))
            {
                _carts[cartId] = new Cart { CartId = cartId };
            }

            var cart = _carts[cartId];
            var previousTotal = cart.TotalPrice;

            // Calcular precio
            var menu = await GetMenu();
            var basePrice = menu.SizePrices[size];
            var toppingsPrice = toppings
                .Select(t => menu.AvailableToppings.First(ti => ti.Topping == t).AdditionalPrice)
                .Sum();

            var unitPrice = basePrice + toppingsPrice;
            var totalPrice = unitPrice * quantity;

            // Crear la nueva pizza
            var newPizza = new Pizza
            {
                Id = _nextPizzaId++,
                Size = size,
                Toppings = toppings,
                Quantity = quantity,
                SpecialInstructions = specialInstructions,
                UnitPrice = unitPrice,
                TotalPrice = totalPrice
            };

            // Agregar al carrito
            cart.Items.Add(newPizza);
            cart.UpdatedAt = DateTime.UtcNow;

            // Recalcular totales del carrito
            UpdateCartTotals(cart);

            var cartDelta = new CartDelta
            {
                AddedPizza = newPizza,
                UpdatedCart = cart,
                PreviousTotal = previousTotal,
                NewTotal = cart.TotalPrice,
                Message = $"Added {quantity} {size} pizza(s) to cart"
            };

            return await Task.FromResult(cartDelta);
        }

        public async Task<RemovePizzaResponse> RemovePizzaFromCart(Guid cartId, int pizzaId)
        {
            if (!_carts.ContainsKey(cartId))
            {
                return await Task.FromResult(new RemovePizzaResponse
                {
                    Success = false,
                    RemovedPizzaId = pizzaId,
                    Message = "Cart not found"
                });
            }

            var cart = _carts[cartId];
            var previousTotal = cart.TotalPrice;
            var pizzaToRemove = cart.Items.FirstOrDefault(p => p.Id == pizzaId);

            if (pizzaToRemove == null)
            {
                return await Task.FromResult(new RemovePizzaResponse
                {
                    Success = false,
                    RemovedPizzaId = pizzaId,
                    UpdatedCart = cart,
                    PreviousTotal = previousTotal,
                    NewTotal = cart.TotalPrice,
                    Message = "Pizza not found in cart"
                });
            }

            cart.Items.Remove(pizzaToRemove);
            cart.UpdatedAt = DateTime.UtcNow;
            UpdateCartTotals(cart);

            return await Task.FromResult(new RemovePizzaResponse
            {
                Success = true,
                RemovedPizzaId = pizzaId,
                UpdatedCart = cart,
                PreviousTotal = previousTotal,
                NewTotal = cart.TotalPrice,
                Message = $"Removed pizza #{pizzaId} from cart"
            });
        }

        public async Task<Pizza?> GetPizzaFromCart(Guid cartId, int pizzaId)
        {
            if (!_carts.ContainsKey(cartId))
            {
                return await Task.FromResult<Pizza?>(null);
            }

            var cart = _carts[cartId];
            Pizza? pizza = cart.Items.FirstOrDefault(p => p.Id == pizzaId);

            return await Task.FromResult(pizza);
        }

        public async Task<Cart> GetCart(Guid cartId)
        {
            if (!_carts.ContainsKey(cartId))
            {
                _carts[cartId] = new Cart { CartId = cartId };
            }

            return await Task.FromResult(_carts[cartId]);
        }

        public async Task<CheckoutResponse> Checkout(Guid cartId, Guid paymentId)
        {
            if (!_carts.ContainsKey(cartId))
            {
                return await Task.FromResult(new CheckoutResponse
                {
                    Success = false,
                    ConfirmationMessage = "Cart not found"
                });
            }

            var cart = _carts[cartId];

            if (cart.Items.Count == 0)
            {
                return await Task.FromResult(new CheckoutResponse
                {
                    Success = false,
                    ConfirmationMessage = "Cart is empty"
                });
            }

            var orderId = Guid.NewGuid();
            var checkoutResponse = new CheckoutResponse
            {
                Success = true,
                OrderId = orderId,
                PaymentId = paymentId,
                Subtotal = cart.Subtotal,
                Tax = cart.Tax,
                TotalAmount = cart.TotalPrice,
                OrderStatus = "Confirmed",
                OrderPlacedAt = DateTime.UtcNow,
                EstimatedDeliveryMinutes = 30 + (cart.Items.Count * 5),
                ConfirmationMessage = $"Order #{orderId} placed successfully! Estimated delivery in 30-45 minutes.",
                OrderedPizzas = new List<Pizza>(cart.Items)
            };

            // Limpiar el carrito después del checkout
            _carts[cartId] = new Cart { CartId = cartId };

            return await Task.FromResult(checkoutResponse);
        }

        private void UpdateCartTotals(Cart cart)
        {
            cart.Subtotal = cart.Items.Sum(p => p.TotalPrice);
            cart.Tax = cart.Subtotal * 0.08m; // 8% tax
            cart.TotalPrice = cart.Subtotal + cart.Tax;
        }
    }

}