using PaymentServiceProvider.Domain.Transactions;

namespace PaymentServiceProvider.Infrastructure.Dto.Payable
{
    public class CreatePayableDto
    {
        public Guid TransactionId { get; set; }
        public decimal TransactionValue { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
