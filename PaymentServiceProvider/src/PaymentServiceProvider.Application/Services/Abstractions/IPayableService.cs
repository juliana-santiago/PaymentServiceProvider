using PaymentServiceProvider.Infrastructure.Dto.Payable;

namespace PaymentServiceProvider.Application.Services.Abstractions
{
    public interface IPayableService
    {
        Task CreatePayableForTransaction(CreatePayableDto createPayableDto);
        Task<PayableDto?> GetListByCardHolderNameAsync(string userName);
    }
}
