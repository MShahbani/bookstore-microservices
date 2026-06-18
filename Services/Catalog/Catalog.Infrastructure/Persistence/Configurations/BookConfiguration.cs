using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(book => book.Id);

        builder.Property(book => book.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(book => book.Author)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(book => book.Price)
            .HasPrecision(18, 0);

        builder.Property(book => book.Stock)
            .IsRequired();
    }
}