using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.ISBN).IsRequired().HasMaxLength(13);
        builder.Property(b => b.BookTitle).IsRequired().HasMaxLength(50);
        builder.Property(b => b.Genre).IsRequired();
        builder.Property(b => b.Description).IsRequired().HasMaxLength(1000);
        builder.Property(b => b.AuthorId).IsRequired();
        builder.Property(b => b.Amount).IsRequired();
    }
}
