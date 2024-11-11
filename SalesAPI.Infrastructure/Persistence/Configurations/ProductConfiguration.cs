using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAPI.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Set up primary key
            builder.HasKey(p => p.Id);

            // Configure properties
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(p => p.Price)
                .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Created).IsRequired();

        }
    }
}