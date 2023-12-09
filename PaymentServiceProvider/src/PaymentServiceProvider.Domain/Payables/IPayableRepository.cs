namespace PaymentServiceProvider.Domain.Payables
{
    public interface IPayableRepository
    {
        Task InsertOnlyParentAsync(Payable transaction);
        Task<List<Payable>> GetListByCardHolderNameAsync(string cardHolderName);
    }
}
