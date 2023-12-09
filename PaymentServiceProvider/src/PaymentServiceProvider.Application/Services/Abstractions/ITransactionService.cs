using PaymentServiceProvider.Infrastructure.Dto.Transaction;

namespace PaymentServiceProvider.Application.Services.Abstractions
{
    public interface ITransactionService
    {
        Task CreateTransactionAsync(CreateTransactionDto registerTransactionRequest);

        Task<IEnumerable<TransactionDto>?> GetAllTransactionsAsync();
    }
}
