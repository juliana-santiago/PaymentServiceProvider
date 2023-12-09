using Microsoft.EntityFrameworkCore;
using PaymentServiceProvider.Domain.Transactions;
using PaymentServiceProvider.Infrastructure.Persistence.Context;

namespace PaymentServiceProvider.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(DataContext context) : base(context) { }
    }
}
