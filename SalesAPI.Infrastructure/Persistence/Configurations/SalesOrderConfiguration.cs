using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Domain.Entities;

namespace SalesAPI.Infrastructure.Persistence.Configurations
{
    public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
    {
        public void Configure(EntityTypeBuilder<SalesOrder> builder)
        {
            // Set up primary key
            builder.HasKey(so => so.Id);

            // Configure properties
            builder.Property(so => so.Created).IsRequired();

            builder.Property(so => so.LastModified)
                   .IsRequired(false);

            builder.Property(so => so.IsDeleted)
                   .HasDefaultValue(false);

            builder.Property(so => so.UnitPrice)
                   .HasColumnType("decimal(18,2)");

            // Configure relationships
            builder.HasOne(so => so.Product)
                   .WithMany()
                   .HasForeignKey(so => so.ProductId)
                   .OnDelete(DeleteBehavior.NoAction); 
        }
    }

}
