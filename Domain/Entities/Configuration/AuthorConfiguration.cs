using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities.Configuration;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).IsRequired().HasMaxLength(30);
        builder.Property(a => a.LastName).IsRequired().HasMaxLength(30);
        builder.Property(a => a.BirthDate).IsRequired().HasMaxLength(10);
        builder.Property(a => a.Country).IsRequired().HasMaxLength(50);
    }
}
