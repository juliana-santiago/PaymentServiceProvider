using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentServiceProvider.Domain.Payables;
using PaymentServiceProvider.Domain.Transactions;

namespace PaymentServiceProvider.Infrastructure.Persistence.Mapping
{
    public class PayableMapping : IEntityTypeConfiguration<Payable>
    {
        public void Configure(EntityTypeBuilder<Payable> builder)
        {
            builder.ToTable("Payables");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(c => c.TransactionId)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.CreateDate)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(c => c.Value)
            .HasColumnType("money")
                .IsRequired();

            builder.HasOne<Transaction>(p => p.Transaction)
                .WithOne(t => t.Payable)
                .HasForeignKey<Payable>(x => x.TransactionId)
                .IsRequired();

            builder.HasOne<PayableType>(p => p.PayableType)
                .WithMany()
                .HasForeignKey(p => p.PayableTypeId);
        }
    }
}
