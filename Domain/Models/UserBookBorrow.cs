namespace Domain.Models;

public class UserBookBorrow
{
    public int Id { get; set; }
    public string UserId { get; set; }  
    public User User { get; set; }
    public int BookId { get; set; }  
    public Book Book { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime ReturnDate { get; set; }
}

