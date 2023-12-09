using Microsoft.Extensions.Logging;
using PaymentServiceProvider.Application.Services;
using PaymentServiceProvider.Application.Services.Abstractions;
using PaymentServiceProvider.Application.Validators;
using PaymentServiceProvider.Domain.Transactions;
using PaymentServiceProvider.Infrastructure.Dto.Payable;
using PaymentServiceProvider.Infrastructure.Dto.Transaction;
using PaymentServiceProvider.Util.Utils;

namespace PaymentServiceProvider.Test.Application
{
    public class TransactionServiceTests
    {
        private readonly Mock<ILogger<TransactionService>> _mockLogger;
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;
        private readonly Mock<IPayableService> _mockPayableService;
        private readonly Mock<CreateTransactionValidator> _mockCreateTransactionValidator;
        private readonly TransactionService _transactionService;

        public TransactionServiceTests()
        {
            _mockLogger = new Mock<ILogger<TransactionService>>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockPayableService = new Mock<IPayableService>();
            _mockCreateTransactionValidator = new Mock<CreateTransactionValidator>();

            _transactionService = new TransactionService(
                _mockLogger.Object,
                _mockTransactionRepository.Object,
                _mockPayableService.Object,
                _mockCreateTransactionValidator.Object);
        }

        [Fact(DisplayName = "CreateTransaction: Should create transaction and payables")]
        public async Task CreateTransaction_Should_Create_Transaction_And_Payables()
        {
            //Arrange
            var cardNumberEncripted = "FSA4FS6F51S6F5SA1FAS5";
            var createTransactionDto = new CreateTransactionDto(
                100,
                "Smartband XYZ 3.0",
                "debit_card",
                "376680816376961",
                "CardHolderName",
                DateTime.Now.AddYears(3).Date.ToString(),
                "145");

            var expirationDate = createTransactionDto.ExpirationDate.ParseToDate();

            var transaction = new Transaction(
                createTransactionDto.PaymentMethod,
                cardNumberEncripted,
                createTransactionDto.CardHolderName,
                expirationDate,
                createTransactionDto.CodeCVC,
                createTransactionDto.TransactionDescription,
                createTransactionDto.TransactionValue);

            _mockPayableService
                .Setup(payableService => payableService
                    .CreatePayableForTransaction(It.IsAny<CreatePayableDto>()));

            _mockTransactionRepository
                .Setup(repository => repository.InsertOnlyParentAsync(transaction));

            //Act
            await _transactionService.CreateTransactionAsync(createTransactionDto);

            //Assert
            _mockTransactionRepository
                .Verify(repository => repository.InsertOnlyParentAsync(It.IsAny<Transaction>()), Times.Once);
            _mockLogger
                .Verify(logger => logger
                    .Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        null,
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>())
                    , Times.Exactly(2));
        }

        [Fact(DisplayName = "CreateTransaction: Should throw when try to create transaction")]
        public void CreateTransaction_Should_Throw_When_Try_To_Create_Transaction()
        {
            //Arrange
            var cardNumberEncripted = "FSA4FS6F51S6F5SA1FAS5";
            var createTransactionDto = new CreateTransactionDto(
                100,
                "Smartband XYZ 3.0",
                "debit_card",
                "376680816376961",
                "CardHolderName",
                DateTime.Now.AddYears(3).Date.ToString(),
                "145");

            _mockPayableService
                .Setup(payableService => payableService
                    .CreatePayableForTransaction(It.IsAny<CreatePayableDto>()));

            _mockTransactionRepository
                .Setup(repository => repository.InsertOnlyParentAsync(It.IsAny<Transaction>()))
                .ThrowsAsync(new Exception("Some message here"));


            //Act
            Func<Task> act = async () => await _transactionService.CreateTransactionAsync(createTransactionDto);

            //Assert
            act.Should().ThrowAsync<Exception>();
            _mockLogger
               .Verify(logger => logger
                   .Log(
                       LogLevel.Error,
                       It.IsAny<EventId>(),
                       It.IsAny<It.IsAnyType>(),
                       null,
                       (Func<It.IsAnyType, Exception, string>)It.IsAny<object>())
                   , Times.Once);
        }

        [Fact(DisplayName = "CreateTransaction: Should throw when try to create payables")]
        public void CreateTransaction_Should_Throw_When_Try_To_Create_payables()
        {
            //Arrange
            var cardNumberEncripted = "FSA4FS6F51S6F5SA1FAS5";
            var createTransactionDto = new CreateTransactionDto(
                100,
                "Smartband XYZ 3.0",
                "debit_card",
                "376680816376961",
                "CardHolderName",
                DateTime.Now.AddYears(3).Date.ToString(),
                "145");

            var expirationDate = createTransactionDto.ExpirationDate.ParseToDate();

            var transaction = new Transaction(
                createTransactionDto.PaymentMethod,
                cardNumberEncripted,
                createTransactionDto.CardHolderName,
                expirationDate,
                createTransactionDto.CodeCVC,
                createTransactionDto.TransactionDescription,
                createTransactionDto.TransactionValue);

            _mockPayableService
                .Setup(payableService => payableService
                    .CreatePayableForTransaction(It.IsAny<CreatePayableDto>()))
                .ThrowsAsync(new Exception("Some message here"));

            _mockTransactionRepository
                .Setup(repository => repository.InsertOnlyParentAsync(transaction));

            //Act
            Func<Task> act = async () => await _transactionService.CreateTransactionAsync(createTransactionDto);

            //Assert
            act.Should().ThrowAsync<Exception>();
            _mockLogger
               .Verify(logger => logger
                   .Log(
                       LogLevel.Error,
                       It.IsAny<EventId>(),
                       It.IsAny<It.IsAnyType>(),
                       null,
                       (Func<It.IsAnyType, Exception, string>)It.IsAny<object>())
                   , Times.Once);
        }

        [Fact(DisplayName = "CreateTransaction: Should throw when try to encrypt cardNumber")]
        public void CreateTransaction_Should_Throw_When_Try_To_Encrypt_CardNumber()
        {
            //Arrange
            var cardNumberEncripted = "FSA4FS6F51S6F5SA1FAS5";
            var createTransactionDto = new CreateTransactionDto(
                100,
                "Smartband XYZ 3.0",
                "debit_card",
                "376680816376961",
                "CardHolderName",
                DateTime.Now.AddYears(3).Date.ToString(),
                "145");

            var expirationDate = createTransactionDto.ExpirationDate.ParseToDate();

            var transaction = new Transaction(
                createTransactionDto.PaymentMethod,
                cardNumberEncripted,
                createTransactionDto.CardHolderName,
                expirationDate,
                createTransactionDto.CodeCVC,
                createTransactionDto.TransactionDescription,
                createTransactionDto.TransactionValue);

            _mockPayableService
                .Setup(payableService => payableService
                    .CreatePayableForTransaction(It.IsAny<CreatePayableDto>()))
                .ThrowsAsync(new Exception("Some message here"));

            _mockTransactionRepository
                .Setup(repository => repository.InsertOnlyParentAsync(transaction));

            //Act
            Func<Task> act = async () => await _transactionService.CreateTransactionAsync(createTransactionDto);

            //Assert
            act.Should().ThrowAsync<Exception>();
            _mockLogger
               .Verify(logger => logger
                   .Log(
                       LogLevel.Error,
                       It.IsAny<EventId>(),
                       It.IsAny<It.IsAnyType>(),
                       null,
                       (Func<It.IsAnyType, Exception, string>)It.IsAny<object>())
                   , Times.Once);
        }

        [Fact(DisplayName = "GetAllTransactionsAsync: Should return all transactions")]
        public async Task GetAllTransactionsAsync_Should_Return_All_Transactions()
        {
            //Arrange
            var transactionsList = new List<Transaction>()
            {
                new Transaction(
                    "debit_card",
                    "376680816376961",
                    "CardHolderName",
                    DateTime.Now.AddYears(3),
                    "142",
                    "Smartband XYZ 3.0",
                    13123),
                new Transaction(
                    "credit_card",
                    "376680816376961",
                    "CardHolderName",
                    DateTime.Now.AddYears(3),
                    "142",
                    "Smartband XYZ 3.0",
                    3213123),
            }; ;

            var listOfTransactionsExpectedResponse = new List<TransactionDto>()
            {
                new TransactionDto(
                    TransactionType.DEBIT_CARD.Id,
                    "6961",
                    "CardHolderName",
                    DateTime.Now.AddYears(3),
                    "142",
                    "Smartband XYZ 3.0",
                    13123),
                new TransactionDto(
                    TransactionType.CREDIT_CARD.Id,
                    "6961",
                    "CardHolderName",
                    DateTime.Now.AddYears(3),
                    "142",
                    "Smartband XYZ 3.0",
                    3213123)
            };

            _mockTransactionRepository
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(transactionsList);

            //Act
            var listOfTransactionsResponse = await _transactionService.GetAllTransactionsAsync();

            //Assert
            listOfTransactionsResponse.Should().BeEquivalentTo(listOfTransactionsExpectedResponse);
        }

        [Fact(DisplayName = "GetAllTransactionsAsync: Should return null")]
        public async Task GetAllTransactionsAsync_Should_Return_Null()
        {
            //Arrange
            _mockTransactionRepository
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(() => null);

            //Act
            var listOfTransactionsResponse = await _transactionService.GetAllTransactionsAsync();

            //Assert
            listOfTransactionsResponse.Should().BeNull();
        }

        [Fact(DisplayName = "GetAllTransactionsAsync: Should throw when try to getAllTransactions")]
        public void GetAllTransactionsAsync_Should_Throw_When_Try_To_GetAllTransactions()
        {
            //Arrange
            _mockTransactionRepository
                .Setup(repository => repository.GetAllAsync())
                .ThrowsAsync(new Exception("Some message here"));

            //Act
            Func<Task> act = async () => await _transactionService.GetAllTransactionsAsync();

            //Assert
            act.Should().ThrowAsync<Exception>();
            _mockLogger
               .Verify(logger => logger
                   .Log(
                       LogLevel.Error,
                       It.IsAny<EventId>(),
                       It.IsAny<It.IsAnyType>(),
                       null,
                       (Func<It.IsAnyType, Exception, string>)It.IsAny<object>())
                   , Times.Once);
        }
    }
}
