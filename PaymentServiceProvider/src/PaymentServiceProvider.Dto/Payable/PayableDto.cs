namespace PaymentServiceProvider.Infrastructure.Dto.Payable
{
    public class PayableDto
    {
        public decimal TotalAvailable { get; set; }
        public decimal TotalWaitingFunds { get; set; }

        public PayableDto(decimal totalAvailable, decimal totalWaintinFunds)
        {
            TotalAvailable = totalAvailable;
            TotalWaitingFunds = totalWaintinFunds;
        }
    }
}
