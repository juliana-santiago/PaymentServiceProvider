using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentServiceProvider.Domain.Payables;

namespace PaymentServiceProvider.Infrastructure.Persistence.Mapping
{
    public class PayableTypeMapping : IEntityTypeConfiguration<PayableType>
    {
        public void Configure(EntityTypeBuilder<PayableType> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(c => c.Description)
                .HasColumnType("varchar(40)")
                .HasColumnName("Name")
                .IsRequired();

            builder.Property(c => c.Fee)
                .HasColumnType("decimal(4,2)")
                .IsRequired();
        }
    }
}
