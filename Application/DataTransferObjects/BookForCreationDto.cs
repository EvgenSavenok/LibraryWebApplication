using Domain.Models;

namespace Application.DataTransferObjects;

public class BookForCreationDto
{
    public string ISBN { get; set; }
    public string BookTitle { get; set; }
    public BookGenre Genre { get; set; }
    public string Description { get; set; }
    public int AuthorId { get; set; }
    public int Amount { get; set; }
}
