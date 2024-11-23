namespace Application.DataTransferObjects;

public class UserBookBorrowDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int BookId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime ReturnDate { get; set; }
}
