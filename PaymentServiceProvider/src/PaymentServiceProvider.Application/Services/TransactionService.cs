using FluentValidation;
using Microsoft.Extensions.Logging;
using PaymentServiceProvider.Application.Services.Abstractions;
using PaymentServiceProvider.Application.Validators;
using PaymentServiceProvider.Domain.Payables;
using PaymentServiceProvider.Domain.Transactions;
using PaymentServiceProvider.Infrastructure.Dto.Payable;
using PaymentServiceProvider.Infrastructure.Dto.Transaction;
using PaymentServiceProvider.Util.Constants;
using PaymentServiceProvider.Util.Utils;

namespace PaymentServiceProvider.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPayableService _payablesService;
        private readonly CreateTransactionValidator _createTransactionValidator;
        private readonly string _logTag;

        public TransactionService(
            ILogger<TransactionService> logger,
            ITransactionRepository transactionsRepository,
            IPayableService payablesService,
            CreateTransactionValidator createTransactionValidator)
        {
            _logger = logger;
            _transactionRepository = transactionsRepository;
            _payablesService = payablesService;
            _createTransactionValidator = createTransactionValidator;

            _logTag = Constants.TagTransactionService;
        }

        public async Task CreateTransactionAsync(CreateTransactionDto createTransactionDto)
        {
            try
            {
                _logger.LogInformation($"{_logTag}[CreateTransactionAsync] called");

                await _createTransactionValidator.ValidateAndThrowAsync(createTransactionDto);

                var expirationDate = createTransactionDto.ExpirationDate.ParseToDate();
                var transactionEntity = new Transaction(
                    createTransactionDto.PaymentMethod,
                    createTransactionDto.CardNumber,
                    createTransactionDto.CardHolderName,
                    expirationDate,
                    createTransactionDto.CodeCVC,
                    createTransactionDto.TransactionDescription,
                    createTransactionDto.TransactionValue);

                await _transactionRepository.InsertOnlyParentAsync(transactionEntity);
                var createPayable = new CreatePayableDto
                {
                    TransactionId = transactionEntity.Id,
                    TransactionType = transactionEntity.TransactionType,
                    TransactionValue = transactionEntity.Value
                };

                await _payablesService.CreatePayableForTransaction(createPayable);

                _logger.LogInformation($"{_logTag}[CreateTransactionAsync][InsertAsync] entity created successfully - {transactionEntity.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_logTag}[CreateTransactionAsync][Error] creating entity - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TransactionDto>?> GetAllTransactionsAsync()
        {
            try
            {
                _logger.LogInformation($"{_logTag}[GetAllTransactionsAsync] called");

                var transactions = await _transactionRepository.GetAllAsync();

                if (transactions == null)
                {
                    _logger.LogInformation($"{_logTag}[GetByIdAsync] return no content");
                    return null;
                }

                var listOfTransactions = new List<TransactionDto>();

                foreach (var transaction in transactions)
                {
                    var transactionDto = new TransactionDto(
                        transaction.TransactionTypeId,
                        transaction.CardNumber,
                        transaction.CardHolderName,
                        transaction.ExpirationDate,
                        transaction.CodeCVC,
                        transaction.Description,
                        transaction.Value);

                    listOfTransactions.Add(transactionDto);
                }

                _logger.LogInformation($"{_logTag}[GetAllAsync] return - {listOfTransactions}");

                return listOfTransactions;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_logTag}[GetAllTransactionsAsync][Error] retrieving list of transactions - {ex.Message}");
                throw;
            }
        }
    }
}
