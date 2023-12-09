using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentServiceProvider.Domain.Payables;
using PaymentServiceProvider.Domain.Transactions;

namespace PaymentServiceProvider.Infrastructure.Persistence.Mapping
{
    public class TransactionMapping : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(c => c.CardNumber)
                .HasColumnType("text")
                .IsRequired();

            builder.Property(c => c.CardHolderName)
                .HasColumnType("varchar(30)")
                .IsRequired();

            builder.Property(c => c.ExpirationDate)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(c => c.CodeCVC)
                .HasColumnType("varchar(4)")
                .IsRequired();

            builder.Property(c => c.Description)
                .HasColumnType("text")
                .IsRequired();

            builder.Property(c => c.Value)
                .HasColumnType("money")
                .IsRequired();

            builder.HasOne<TransactionType>(tt => tt.TransactionType)
                .WithMany()
                .HasForeignKey(tt => tt.TransactionTypeId);
        }
    }
}