using Application.RequestFeatures;

namespace Domain.Entities.RequestFeatures;

public class AuthorParameters : RequestParameters
{
    public string SearchTerm { get; set; } = null!;
}
