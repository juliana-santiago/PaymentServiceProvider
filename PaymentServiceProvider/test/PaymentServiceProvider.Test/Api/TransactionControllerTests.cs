using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PaymentServiceProvider.Application.Services.Abstractions;
using PaymentServiceProvider.Domain.Transactions;
using PaymentServiceProvider.Infrastructure.Dto.Payable;
using PaymentServiceProvider.Infrastructure.Dto.Transaction;
using PaymentServiceProvider.Util.Utils;
using System.Net;
using System.Text;

namespace PaymentServiceProvider.Test.Api
{
    public class TransactionControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _webApplicationFactory;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly Mock<IPayableService> _mockPayableService;
        private readonly Mock<ITransactionService> _mockTransactionService;
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;

        public TransactionControllerTests(CustomWebApplicationFactory webApplicationFactory)
        {
            _mockPayableService = new Mock<IPayableService>();
            _mockTransactionService = new Mock<ITransactionService>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _webApplicationFactory = webApplicationFactory;

            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            _serializerSettings.Converters.Add(new StringEnumConverter());
            _serializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            _serializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        [Fact(DisplayName = "CreateTransactionAsync: Should create transaction")]
        public async void CreateTransactionAsync_Should_Create_Transaction()
        {
            //Arrange
            var cardNumberEncripted = "FHDAU164DF6A4845F1AD";
            var createTransactionDto = new CreateTransactionDto(
                100,
                "Smartband XYZ 3.0",
                "debit_card",
                "376680816376961",
                "cardHolderName",
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

            var createPayableDto = new CreatePayableDto
            {
                TransactionId = transaction.Id,
                TransactionType = transaction.TransactionType,
                TransactionValue = transaction.Value
            };

            _mockPayableService
                .Setup(payableService => payableService
                    .CreatePayableForTransaction(createPayableDto));

            _mockTransactionRepository
                .Setup(repository => repository
                    .InsertOnlyParentAsync(transaction));

            _mockTransactionService
                .Setup(transactionService => transactionService
                    .CreateTransactionAsync(createTransactionDto));

            var client = _webApplicationFactory.CreateHttpClient(services =>
            {
                services.AddTransient(mockService => _mockPayableService.Object);
                services.AddTransient(mockService => _mockTransactionRepository.Object);
                services.AddTransient(mockService => _mockTransactionService.Object);
            });

            var endpoint = new Uri("api/v1/transactions", UriKind.Relative);

            //Act
            var jsonRequest = JsonConvert.SerializeObject(createTransactionDto);
            var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(endpoint, stringContent);

            //Assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact(DisplayName = "CreateTransactionAsync: Should Return BadRequest when try to create transaction")]
        public async void CreateTransactionAsync_Should_Return_BadRequest_When_Try_To_Create_Transaction()
        {
            //Arrange
            var createTransactionDto = new CreateTransactionDto
            {
                TransactionValue = 3432
            };

            var expirationDate = createTransactionDto.ExpirationDate.ParseToDate();

            _mockTransactionService
                .Setup(transactionService => transactionService
                    .CreateTransactionAsync(createTransactionDto));

            var client = _webApplicationFactory.CreateHttpClient(services =>
            {
                services.AddTransient(mockService => _mockPayableService.Object);
                services.AddTransient(mockService => _mockTransactionRepository.Object);
                services.AddTransient(mockService => _mockTransactionService.Object);
            });

            var endpoint = new Uri("api/v1/transactions", UriKind.Relative);

            //Act
            var jsonRequest = JsonConvert.SerializeObject(createTransactionDto);
            var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(endpoint, stringContent);

            //Assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "GetAllTransactionsAsync: Should return all transactions")]
        public async void GetAllTransactionsAsync_Should_Return_All_Transactions()
        {
            //Arrange
            var transactionsList = new List<Transaction>()
            {
                new Transaction(
                    "debit_card",
                    "FDASF5F1AS5F4FDA5SF46FA5",
                    "cardHolderName",
                    DateTime.Now.AddYears(3),
                    "142",
                    "Smartband XYZ 3.0",
                    1231),
                new Transaction(
                    "credit_card",
                    "FDASF5F1AS5F4FDA5SF46FA5",
                    "cardHolderName",
                    DateTime.Now.AddYears(3),
                    "142",
                    "Smartband XYZ 3.0",
                    13134),
            }; ;

            var transactionsDtoList = new List<TransactionDto>()
            {
                new TransactionDto(
                    TransactionType.DEBIT_CARD.Id,
                    "6961",
                    "cardHolderName",
                    DateTime.Now.AddYears(3),
                    "142",
                    "Smartband XYZ 3.0",
                    1231),
                new TransactionDto(
                    TransactionType.CREDIT_CARD.Id,
                    "6961",
                    "cardHolderName",
                    DateTime.Now.AddYears(3),
                    "142",
                    "Smartband XYZ 3.0",
                    13134)
            };

            _mockTransactionRepository
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(transactionsList);

            _mockTransactionService
                .Setup(service => service.GetAllTransactionsAsync())
                .ReturnsAsync(transactionsDtoList);

            var client = _webApplicationFactory.CreateHttpClient(services =>
            {
                services.AddTransient(mockService => _mockPayableService.Object);
                services.AddTransient(mockService => _mockTransactionRepository.Object);
                services.AddTransient(mockService => _mockTransactionService.Object);
            });

            var endpoint = new Uri("api/v1/transactions", UriKind.Relative);

            //Act
            var getResponse = await client.GetAsync(endpoint);

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "GetAllTransactionsAsync: Should return NoContent when has none transactions")]
        public async void GetAllTransactionsAsync_Should_Return_NoContent_When_Has_None_Transactions()
        {
            //Arrange
            _mockTransactionRepository
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(() => null);

            _mockTransactionService
                .Setup(service => service.GetAllTransactionsAsync())
                .ReturnsAsync(() => null);

            var client = _webApplicationFactory.CreateHttpClient(services =>
            {
                services.AddTransient(mockService => _mockPayableService.Object);
                services.AddTransient(mockService => _mockTransactionRepository.Object);
                services.AddTransient(mockService => _mockTransactionService.Object);
            });

            var endpoint = new Uri("api/v1/transactions", UriKind.Relative);

            //Act
            var getResponse = await client.GetAsync(endpoint);

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
