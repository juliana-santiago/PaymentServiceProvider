namespace PaymentServiceProvider.Domain.Transactions
{
    public interface ITransactionRepository
    {
        Task InsertOnlyParentAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAllAsync();
    }
}
