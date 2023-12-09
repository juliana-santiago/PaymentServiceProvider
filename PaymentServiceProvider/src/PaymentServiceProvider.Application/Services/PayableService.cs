using Microsoft.Extensions.Logging;
using PaymentServiceProvider.Application.Services.Abstractions;
using PaymentServiceProvider.Domain.Payables;
using PaymentServiceProvider.Infrastructure.Dto.Payable;
using PaymentServiceProvider.Util.Constants;

namespace PaymentServiceProvider.Application.Services
{
    public class PayableService : IPayableService
    {
        private readonly ILogger<PayableService> _logger;
        private readonly IPayableRepository _payableRepository;
        private readonly string _logTag;

        public PayableService(
            ILogger<PayableService> logger,
            IPayableRepository payableRepository)
        {
            _logger = logger;
            _payableRepository = payableRepository;

            _logTag = Constants.TagPayableService;
        }

        public async Task CreatePayableForTransaction(CreatePayableDto createPayableDto)
        {
            try
            {
                _logger.LogInformation($"{_logTag}[CreatePayableForTransaction] called - {createPayableDto.TransactionId}");

                var payableEntity = new Payable(
                    createPayableDto.TransactionType,
                    createPayableDto.TransactionValue,
                    createPayableDto.TransactionId);

                await _payableRepository.InsertOnlyParentAsync(payableEntity);

                _logger.LogInformation($"{_logTag}[CreatePayableForTransaction] entity created successfully - {payableEntity.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_logTag}[CreatePayableForTransaction][Error] creating entity  - {ex.Message}");
                throw;
            }
        }

        public async Task<PayableDto?> GetListByCardHolderNameAsync(string cardHolderName)
        {
            try
            {
                _logger.LogInformation($"{_logTag}[GetListByCardHolderNameAsync] called - {cardHolderName}");

                var payables = await _payableRepository.GetListByCardHolderNameAsync(cardHolderName);

                if (payables == null)
                {
                    return null;
                }

                var totalAvailable = payables
                    .Where(p => p.PayableTypeId == PayableType.PAID.Id)
                    .Sum(p => p.Value);

                var totalWaitingFunds = payables
                    .Where(p => p.PayableTypeId == PayableType.WAITING_FUNDS.Id)
                    .Sum(p => p.Value);

                _logger.LogInformation($"{_logTag}[GetListByCardHolderNameAsync] return sum of payable list wiht: {payables.Count}payables");

                return new PayableDto(totalAvailable, totalWaitingFunds);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_logTag}[GetPayableById][Error] retrieving specific payable - {ex.Message}");
                throw;
            }
        }
    }
}
