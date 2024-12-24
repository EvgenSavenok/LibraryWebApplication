using System.Text.Json.Serialization;

namespace Domain.Models;

public enum BookGenre
{
    Adventures = 1, 
    LoveStories = 2,
    Horrors = 3,
    FairyTales = 4
}

public class Book
{
    public int Id { get; set; }
    public string ISBN { get; set; }
    public string BookTitle { get; set; }
    public BookGenre Genre { get; set; }
    public string Description { get; set; }
    public int AuthorId { get; set; }  
    public Author Author { get; set; } 
    [JsonIgnore]
    public virtual ICollection<UserBookBorrow> UserBookBorrows { get; set; } = new List<UserBookBorrow>();
    public int Amount { get; set; }
}
