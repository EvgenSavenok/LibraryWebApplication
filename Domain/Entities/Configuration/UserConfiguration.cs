using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(30);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(30);
        builder.Property(u => u.UserName).IsRequired().HasMaxLength(30);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(30);
        builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(30);
    }
}
