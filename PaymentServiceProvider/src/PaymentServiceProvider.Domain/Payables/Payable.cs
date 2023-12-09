using PaymentServiceProvider.Domain.Transactions;

namespace PaymentServiceProvider.Domain.Payables
{
    public class Payable
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; private set; }
        public decimal Value { get; private set; }
        public short PayableTypeId { get; private set; }
        public virtual PayableType PayableType { get; private set; }
        public Guid TransactionId { get; private set; }
        public virtual Transaction Transaction { get; private set; }

        protected Payable() { }

        public Payable(
            TransactionType transactionType,
            decimal transactionValue,
            Guid transactionId)
        {
            Id = Guid.NewGuid();
            TransactionId = transactionId;
            PayableType = transactionType == TransactionType.CREDIT_CARD
                ? PayableType.WAITING_FUNDS
                : PayableType.PAID;
            PayableTypeId = PayableType.Id;
            CreateDate = this.PayableType == PayableType.PAID
                ? DateTime.Now.Date
                : DateTime.Now.Date.AddDays(30);

            Value = transactionValue * this.PayableType.Fee;
        }
    }
}
