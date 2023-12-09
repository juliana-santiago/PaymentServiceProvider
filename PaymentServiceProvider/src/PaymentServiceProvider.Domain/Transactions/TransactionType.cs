using PaymentServiceProvider.Domain.Base;

namespace PaymentServiceProvider.Domain.Transactions
{
    public class TransactionType : Enumeration<short>, IValueObject<TransactionType>
    {
        public static readonly TransactionType CREDIT_CARD = new(1, "credit_card");
        public static readonly TransactionType DEBIT_CARD = new(2, "debit_card");

        public bool SameValueAs(TransactionType other)
        {
            return Equals(other);
        }

        #region Constructors
        public TransactionType(short id, string description) 
            : base(id, description) { }

        protected TransactionType() : base(0, string.Empty)
        {
            // Required by ORM
        }
        #endregion
    }
}
