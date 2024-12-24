using Domain.Entities.RequestFeatures;
using Domain.Models;

namespace Application.RequestFeatures;

public class BookParameters : RequestParameters
{
    public BookGenre Genre { get; set; }
    public int AuthorId { get; set; }
    public string SearchTerm { get; set; } = null!;
}
