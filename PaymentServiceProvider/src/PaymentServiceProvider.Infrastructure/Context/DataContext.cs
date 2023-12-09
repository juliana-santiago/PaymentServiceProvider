using Microsoft.EntityFrameworkCore;
using PaymentServiceProvider.Domain.Payables;
using PaymentServiceProvider.Domain.Transactions;
using PaymentServiceProvider.Infrastructure.Persistence.Mapping;

namespace PaymentServiceProvider.Infrastructure.Persistence.Context
{
    public class DataContext : DbContext
    {
        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
       
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Payable> Payables { get; set; }
        public DbSet<PayableType> PayableTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PayableMapping).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
