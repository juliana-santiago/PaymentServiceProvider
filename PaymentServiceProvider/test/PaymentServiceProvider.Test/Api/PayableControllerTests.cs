using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PaymentServiceProvider.Application.Services.Abstractions;
using PaymentServiceProvider.Domain.Payables;
using PaymentServiceProvider.Domain.Transactions;
using PaymentServiceProvider.Infrastructure.Dto.Payable;
using System.Net;
using System.Text;

namespace PaymentServiceProvider.Test.Api
{
    public class PayableControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _webApplicationFactory;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly Mock<IPayableService> _mockPayableService;
        private readonly Mock<IPayableRepository> _mockPayableRepository;

        public PayableControllerTests(CustomWebApplicationFactory webApplicationFactory)
        {
            _mockPayableService = new Mock<IPayableService>();
            _mockPayableRepository = new Mock<IPayableRepository>();
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

        [Fact(DisplayName = "GetPayablesByCardHolderName: Should return payables by cardHolderName")]
        public async void GetPayablesByCardHolderName_Should_Return_Payables_By_CardHolderName()
        {
            //Arrange
            var listOfPayables = new List<Payable>()
            {
                new Payable(TransactionType.CREDIT_CARD, 95, Guid.NewGuid()),
                new Payable(TransactionType.CREDIT_CARD, 95, Guid.NewGuid()),
                new Payable(TransactionType.CREDIT_CARD, 95, Guid.NewGuid()),
                new Payable(TransactionType.DEBIT_CARD, 97, Guid.NewGuid()),
                new Payable(TransactionType.DEBIT_CARD, 97, Guid.NewGuid()),
                new Payable(TransactionType.DEBIT_CARD, 97, Guid.NewGuid()),
                new Payable(TransactionType.CREDIT_CARD, 95, Guid.NewGuid()),
                new Payable(TransactionType.DEBIT_CARD, 97, Guid.NewGuid()),
            };
            var payableResponse = new PayableDto(380, 388);

            _mockPayableRepository
                .Setup(repository => repository
                    .GetListByCardHolderNameAsync(It.IsAny<string>()))
                .ReturnsAsync(listOfPayables);

            _mockPayableService
                .Setup(service => service
                    .GetListByCardHolderNameAsync(It.IsAny<string>()))
                .ReturnsAsync(payableResponse);

            var client = _webApplicationFactory.CreateHttpClient(services =>
            {
                services.AddTransient(mockService => _mockPayableService.Object);
                services.AddTransient(mockService => _mockPayableRepository.Object);
            });

            var endpoint = new Uri("api/v1/payables/{cardHolderName}", UriKind.Relative);

            //Act
            var jsonRequest = JsonConvert.SerializeObject("cardHolderName");
            var stringContent = new StringContent(
                jsonRequest,
                Encoding.UTF8,
                "application/json");
            var getResponse = await client.GetAsync(endpoint);

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "GetPayablesByCardHolderName: Should return NoContent when cardHolderName has not payables")]
        public async void GetPayablesByCardHolderName_Should_Return_NoContent_When_CardHolderName_Has_Not_Payables()
        {
            //Arrange
            var listOfPayables = new List<Payable>()
            {
                new Payable(TransactionType.CREDIT_CARD, 95, Guid.NewGuid()),
                new Payable(TransactionType.CREDIT_CARD, 95, Guid.NewGuid()),
                new Payable(TransactionType.CREDIT_CARD, 95, Guid.NewGuid()),
                new Payable(TransactionType.DEBIT_CARD, 97, Guid.NewGuid()),
                new Payable(TransactionType.DEBIT_CARD, 97, Guid.NewGuid()),
                new Payable(TransactionType.DEBIT_CARD, 97, Guid.NewGuid()),
                new Payable(TransactionType.CREDIT_CARD, 95, Guid.NewGuid()),
                new Payable(TransactionType.DEBIT_CARD, 97, Guid.NewGuid()),
            };
            var payableResponse = new PayableDto(380, 388);

            _mockPayableRepository
                .Setup(repository => repository
                    .GetListByCardHolderNameAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _mockPayableService
                .Setup(service => service
                    .GetListByCardHolderNameAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var client = _webApplicationFactory.CreateHttpClient(services =>
            {
                services.AddTransient(mockService => _mockPayableService.Object);
                services.AddTransient(mockService => _mockPayableRepository.Object);
            });

            var endpoint = new Uri("api/v1/payables/{cardHolderName}", UriKind.Relative);

            //Act
            var jsonRequest = JsonConvert.SerializeObject("cardHolderName");
            var stringContent = new StringContent(
                jsonRequest,
                Encoding.UTF8,
                "application/json");
            var getResponse = await client.GetAsync(endpoint);

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

    }
}
