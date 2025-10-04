namespace SemanticTest
{
    public interface IPaymentService
    {
        Task<Guid> RequestPaymentFromUserAsync(Guid cartId);
    }

}
