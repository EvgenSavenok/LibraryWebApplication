using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities.Configuration;

public class BorrowConfiguration : IEntityTypeConfiguration<UserBookBorrow>
{
    public void Configure(EntityTypeBuilder<UserBookBorrow> builder)
    {
        builder.HasKey(ub => ub.Id);
        builder.Property(ub => ub.UserId).IsRequired();
        builder.HasOne(ub => ub.User)
            .WithMany(u => u.UserBookBorrows) 
            .HasForeignKey(ub => ub.UserId);
        builder.HasOne(ub => ub.Book)
            .WithMany(b => b.UserBookBorrows) 
            .HasForeignKey(ub => ub.BookId);
        builder.Property(ub => ub.BorrowDate).IsRequired();
        builder.Property(ub => ub.ReturnDate).IsRequired();
    }
}
