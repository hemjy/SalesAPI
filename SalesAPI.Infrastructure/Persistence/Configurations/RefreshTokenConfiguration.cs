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
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // Table name configuration
            builder.ToTable("RefreshTokens");

            // Primary Key configuration
            builder.HasKey(rt => rt.Id);


            builder.Property(rt => rt.UserId)
                .IsRequired();

            builder.Property(rt => rt.Token)
                .IsRequired();

            builder.Property(rt => rt.ExpirationDate)
                .IsRequired();

            builder.Property(rt => rt.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(rt => rt.Created)
                .IsRequired();

            builder.Property(rt => rt.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(rt => rt.Modified)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(rt => rt.LastModified)
                .IsRequired(false);

            builder.Property(rt => rt.CreatedBy)
                .HasMaxLength(255);

            builder.Property(rt => rt.ModifiedBy)
                .HasMaxLength(255);

            // Optional: Define any relationships if needed
            // For example, linking RefreshToken to a User entity:
            builder.HasOne(rt => rt.User)
                   .WithMany()
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
