using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(order => order.Id);

        builder.Property(order => order.BookId)
            .IsRequired();

        builder.Property(order => order.Quantity)
            .IsRequired();

        builder.Property(order => order.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
    }
}