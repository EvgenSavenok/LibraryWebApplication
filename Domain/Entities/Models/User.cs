using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Models;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public virtual ICollection<UserBookBorrow> UserBookBorrows { get; set; } = new List<UserBookBorrow>();
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; }
}
