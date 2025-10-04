namespace SemanticTest
{
    public interface IUserContext
    {
        Guid GetCartId();
        Task<Guid> GetCartIdAsync();
    }

}
