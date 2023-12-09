using PaymentServiceProvider.Domain.Base;
using PaymentServiceProvider.Domain.Payables;
using System.Transactions;

namespace PaymentServiceProvider.Domain.Transactions
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string CardNumber { get; private set; }
        public string CardHolderName { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public string CodeCVC { get; private set; }
        public string Description { get; private set; }
        public decimal Value { get; private set; }

        public virtual Payable Payable { get; private set; }
        public virtual TransactionType? TransactionType { get; private set; }
        public short TransactionTypeId { get; private set; }

        protected Transaction() { }

        public Transaction(
            string transactionType,
            string cardNumber,
            string cardHolderName,
            DateTime expirationDate,
            string codeCVC,
            string description,
            decimal value)
        {
            Id = Guid.NewGuid();
            TransactionType = transactionType == TransactionType.DEBIT_CARD.Description
                ? TransactionType.DEBIT_CARD
                : transactionType == TransactionType.CREDIT_CARD.Description
                    ? TransactionType.CREDIT_CARD : throw new ApplicationException("Tipo de transação inválida");
            TransactionTypeId = TransactionType.Id;
            CardNumber = cardNumber.Substring(cardNumber.Length - 4);
            CardHolderName = cardHolderName;
            ExpirationDate = expirationDate.Date;
            CodeCVC = codeCVC;
            Description = description;
            Value = value;
        }
    }
}

