using Microsoft.EntityFrameworkCore;
using PaymentServiceProvider.Domain.Payables;
using PaymentServiceProvider.Infrastructure.Persistence.Context;

namespace PaymentServiceProvider.Infrastructure.Persistence.Repositories
{
    public class PayableRepository : BaseRepository<Payable>, IPayableRepository
    {
        public PayableRepository(DataContext context) : base(context) { }

        public async Task<List<Payable>> GetListByCardHolderNameAsync(string cardHolderName)
        {
            var payables = await this.Table
                .AsNoTracking()
                .Include(t => t.Transaction)
                .Where(payable => payable.Transaction.CardHolderName.ToLower() == cardHolderName.ToLower())
                .ToListAsync();

            return payables;
        }
    }
}
