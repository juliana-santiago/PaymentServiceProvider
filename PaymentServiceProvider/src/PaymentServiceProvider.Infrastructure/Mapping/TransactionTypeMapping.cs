using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentServiceProvider.Domain.Transactions;

namespace PaymentServiceProvider.Infrastructure.Persistence.Mapping
{
    public class TransactionTypeMapping : IEntityTypeConfiguration<TransactionType>
    {
        public void Configure(EntityTypeBuilder<TransactionType> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(c => c.Description)
                .HasColumnType("varchar(40)")
                .HasColumnName("Name")
                .IsRequired();
        }
    }
}
