using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentServiceProvider.Domain.Payables;
using PaymentServiceProvider.Domain.Transactions;
using PaymentServiceProvider.Infrastructure.Persistence.Context;
using PaymentServiceProvider.Infrastructure.Persistence.Repositories;

namespace PaymentServiceProvider.Test.Persistence
{
    public class PayableRepositoryTests
    {
        public DataContext _dbContext { get; set; }
        public PayableRepository _payableRepository { get; set; }
        public List<Transaction> _transactions { get; set; }

        public PayableRepositoryTests ()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "PaymentServiceProviderDb")
                .UseInternalServiceProvider(serviceProvider)
                .EnableSensitiveDataLogging();

            _dbContext = new DataContext(builder.Options);
            _payableRepository =  new PayableRepository(_dbContext);

            _transactions = new List<Transaction>()
            {
                new Transaction(
                    "credit_card",
                    "encryptedCardNumber",
                    "cardHolderName", DateTime.Now.AddYears(3),
                    "145",
                    "Smartband XYZ 3.0",
                    45156),
                new Transaction(
                    "debit_card",
                    "encryptedCardNumber",
                    "cardHolderName",
                    DateTime.Now.AddYears(3),
                    "181",
                    "Smartband XYZ 4.0",
                    1561)
            };
        
            foreach(var transaction in _transactions)
            {
                _dbContext.Add(transaction);  
            }
        }

        [Fact(DisplayName = "GetListByCardHolderNameAsync: Should return list of payables by cardHolderName")]
        public async Task GetListByCardHolderNameAsync_Should_Return_List_Of_Payables_By_CardHolderNName()
        {
            // Arrange
            var listOfPayables = new List<Payable>()
            {
                new Payable(
                    TransactionType.CREDIT_CARD, 
                    100, 
                    _transactions.FirstOrDefault().Id),
                new Payable(
                    TransactionType.DEBIT_CARD, 
                    100, 
                    _transactions.LastOrDefault().Id),
            };

            var repository = new PayableRepository(_dbContext);

            foreach (var payable in listOfPayables)
            {
                await repository.InsertOnlyParentAsync(payable);
            }

            // Act
            var listOfPayablesResponse = await _payableRepository.GetListByCardHolderNameAsync("cardHolderName");

            // Assert
            listOfPayablesResponse.Count.Should().Be(2);
        }

        [Fact(DisplayName = "GetListByCardHolderNameAsync: Should return none when the cardHolderName has nothing to receive")]
        public async Task GetListByCardHolderNameAsync_Should_Return_None_When_CardHolderName_Has_Nothing_To_Receive()
        {
            // Arrange

            // Act
            var listOfPayablesResponse = await _payableRepository.GetListByCardHolderNameAsync("cardHolderName");

            // Assert
            listOfPayablesResponse.Should().BeEmpty();
            listOfPayablesResponse.Count.Should().Be(0);
        }
    }
}
