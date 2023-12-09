using Microsoft.Extensions.Logging;
using PaymentServiceProvider.Application.Services;
using PaymentServiceProvider.Application.Services.Abstractions;
using PaymentServiceProvider.Domain.Payables;
using PaymentServiceProvider.Domain.Transactions;
using PaymentServiceProvider.Infrastructure.Dto.Payable;

namespace PaymentServiceProvider.Test.Application
{
    public class PayableServiceTests
    {
        private readonly Mock<ILogger<PayableService>> _mockLogger;
        private readonly Mock<IPayableRepository> _mockPayableRepository;
        private readonly IPayableService _payableService;

        public PayableServiceTests()
        {
            _mockLogger = new Mock<ILogger<PayableService>>();
            _mockPayableRepository = new Mock<IPayableRepository>();

            _payableService = new PayableService(_mockLogger.Object, _mockPayableRepository.Object);
        }

        [Fact(DisplayName = "GetListByCardHolderNameAsync: Should return PayableDto")]
        public async Task GetListByCardHolderNameAsync_Should_Return_GetPayableDto()
        {
            //Arrange
            var listOfPayables = new List<Payable>()
            {
                new Payable(TransactionType.CREDIT_CARD, 100, Guid.NewGuid()),
                new Payable(TransactionType.CREDIT_CARD, 100, Guid.NewGuid()),
                new Payable(TransactionType.CREDIT_CARD, 100, Guid.NewGuid()),
                new Payable(TransactionType.CREDIT_CARD, 100, Guid.NewGuid()),
                new Payable(TransactionType.DEBIT_CARD, 100, Guid.NewGuid()),
                new Payable(TransactionType.DEBIT_CARD, 100, Guid.NewGuid()),
                new Payable(TransactionType.DEBIT_CARD, 100, Guid.NewGuid()),
                new Payable(TransactionType.DEBIT_CARD, 100, Guid.NewGuid()),
            };

            var payableExpectedResponse = new PayableDto(388, 380);

            _mockPayableRepository
                .Setup(repository => repository
                    .GetListByCardHolderNameAsync(It.IsAny<string>()))
                .ReturnsAsync(listOfPayables);

            //Act
            var payableResponse = await _payableService.GetListByCardHolderNameAsync("cardHolderName");

            //Assert
            payableResponse.Should().BeEquivalentTo(payableExpectedResponse);
        }

        [Fact(DisplayName = "GetListByCardHolderNameAsync: Should throw when try to get PayableDto")]
        public void GetListByCardHolderNameAsync_Should_Throw_When_Try_To_GetPayableDto()
        {
            //Arrange
            _mockPayableRepository
                .Setup(repository => repository
                    .GetListByCardHolderNameAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Some message here"));

            //Act
            Func<Task> act = async () => await _payableService.GetListByCardHolderNameAsync(It.IsAny<string>());

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

        [Fact(DisplayName = "GetListByCardHolderNameAsync: Should return null")]
        public async Task GetListByCardHolderNameAsyncGetByIdAsync_Should_Return_Null()
        {
            //Arrange
            _mockPayableRepository
                .Setup(repository => repository
                    .GetListByCardHolderNameAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            //Act
            var payableResponse = await _payableService.GetListByCardHolderNameAsync(It.IsAny<string>());

            //Assert
            payableResponse.Should().BeNull();
        }
    }
}
