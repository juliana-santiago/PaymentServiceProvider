using PaymentServiceProvider.Domain.Base;

namespace PaymentServiceProvider.Domain.Payables
{
    public class PayableType : Enumeration<short>, IValueObject<PayableType>
    {
        public static readonly PayableType WAITING_FUNDS = new(1, "waiting_funds", 0.95M);
        public static readonly PayableType PAID = new(2, "paid", 0.97M);

        public decimal Fee { get; set; }

        public bool SameValueAs(PayableType other)
        {
            return Equals(other);
        }

        #region Constructors
        protected PayableType() : base(0, string.Empty)
        {
            // Required by ORM
        }

        public PayableType(short id, string description, decimal fee)
            : base(id, description)
        {
            this.Fee = fee;
        }
        #endregion
    }
}
