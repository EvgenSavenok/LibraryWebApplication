namespace Application.DataTransferObjects;

public class PageDataDto
{
    public List<GenreDto> Genres { get; set; }
    public IEnumerable<AuthorDto> Authors { get; set; }
    public BookDto Book { get; set; }
    public AuthorDto Author { get; set; }
}
