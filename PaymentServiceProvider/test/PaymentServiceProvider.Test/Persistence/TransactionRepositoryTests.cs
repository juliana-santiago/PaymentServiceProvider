using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentServiceProvider.Domain.Transactions;
using PaymentServiceProvider.Infrastructure.Persistence.Context;
using PaymentServiceProvider.Infrastructure.Persistence.Repositories;

namespace PaymentServiceProvider.Test.Persistence
{
    public class TransactionRepositoryTests
    {
        public DataContext _dbContext { get; set; }

        public ITransactionRepository CreateTransactionRepository()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "PaymentServiceProviderDb")
                .UseInternalServiceProvider(serviceProvider)
                .EnableSensitiveDataLogging();

            _dbContext = new DataContext(builder.Options);
            return new TransactionRepository(_dbContext);
        }

        [Fact(DisplayName = "GetAllAsync: Should return list of Transactions")]
        public async Task GetAllAsync_Should_Return_List_Of_Transactions()
        {
            // Arrange
            var listOfTransactions = new List<Transaction>()
            {
                new Transaction(
                    TransactionType.CREDIT_CARD.Description,
                    "6961",
                    "cardHolderName", 
                    DateTime.Now.AddYears(3),
                    "145",
                    "Smartband XYZ 3.0",
                    45156),
                new Transaction(
                    TransactionType.DEBIT_CARD.Description,
                    "6961",
                    "cardHolderName",
                    DateTime.Now.AddYears(3),
                    "181",
                    "Smartband XYZ 4.0",
                    1561)
            }.AsEnumerable();

            var transactionRepository = CreateTransactionRepository();

            foreach (var transaction in listOfTransactions)
            {
                await transactionRepository.InsertOnlyParentAsync(transaction);
            }

            // Act
            var listOfTransactionsResponse = await transactionRepository.GetAllAsync();

            // Assert
            listOfTransactionsResponse.FirstOrDefault().Id.Should().Be(listOfTransactions.FirstOrDefault().Id);
            listOfTransactionsResponse.LastOrDefault().Id.Should().Be(listOfTransactions.LastOrDefault().Id);
            listOfTransactionsResponse.Count().Should().Be(2);
        }

        [Fact(DisplayName = "GetAllAsync: Should return none when try to get list of Transactions")]
        public async Task GetAllAsync_Should_Return_None_When_Try_To_Get_List_Of_Transactions()
        {
            // Arrange
            var transactionRepository = CreateTransactionRepository();

            // Act
            var listOfTransactionsResponse = await transactionRepository.GetAllAsync();

            // Assert
            listOfTransactionsResponse.Should().BeEmpty();
            listOfTransactionsResponse.ToList().Count.Should().Be(0);
        }
    }
}
