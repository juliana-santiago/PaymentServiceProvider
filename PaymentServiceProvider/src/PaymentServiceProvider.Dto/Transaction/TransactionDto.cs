using PaymentServiceProvider.Domain.Transactions;

namespace PaymentServiceProvider.Infrastructure.Dto.Transaction
{
    public class TransactionDto
    {
        public int TransactionType { get; private set; }
        public string CardNumber { get; private set; }
        public string CardHolderNamer { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public string CodeCVC { get; private set; }
        public string Description { get; private set; }
        public decimal Value { get; private set; }

        public TransactionDto(
            int transactionType,
            string cardNumber,
            string cardHolderName,
            DateTime expirationDate,
            string codeCVC,
            string description,
            decimal value)
        {
            TransactionType = transactionType;
            CardNumber = cardNumber;
            CardHolderNamer = cardHolderName;
            ExpirationDate = expirationDate.Date;
            CodeCVC = codeCVC;
            Description = description;
            Value = value;
        }
    }
}
