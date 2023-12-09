namespace PaymentServiceProvider.Infrastructure.Dto.Transaction
{
    public class CreateTransactionDto
    {
        public decimal TransactionValue { get; set; }
        public string TransactionDescription { get; set; }
        public string PaymentMethod { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpirationDate { get; set; }
        public string CodeCVC { get; set; }

        public CreateTransactionDto() { }

        public CreateTransactionDto(
            decimal transactionValue,
            string transactionDescription,
            string paymantMethod,
            string cardNumber,
            string cardHolderName,
            string expirationDate,
            string codeCVC)
        {
            TransactionValue = transactionValue;
            TransactionDescription = transactionDescription;
            PaymentMethod = paymantMethod;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            ExpirationDate = expirationDate;
            CodeCVC = codeCVC;
        }
    }
}