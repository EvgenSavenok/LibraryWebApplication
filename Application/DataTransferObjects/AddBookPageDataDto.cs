namespace Application.DataTransferObjects;

public class AddBookPageDataDto
{
    public List<GenreDto> Genres { get; set; }
    public IEnumerable<AuthorDto> Authors { get; set; }
    public BookDto Book { get; set; }
}
