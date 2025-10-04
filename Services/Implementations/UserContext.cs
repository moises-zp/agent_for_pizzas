namespace SemanticTest
{
    public class UserContext : IUserContext
    {
        private Guid _cartId = Guid.NewGuid();

        public Guid GetCartId() => _cartId;

        public Task<Guid> GetCartIdAsync() => Task.FromResult(_cartId);
    }

}