using Application.DataTransferObjects;
using Domain.Entities.RequestFeatures;

namespace Application.Contracts.ServicesContracts;

public interface IAuthorService
{
    Task<PagedResult<AuthorDto>> GetAllAuthorsAsync(AuthorParameters requestParameters);
    Task<AuthorDto> GetAuthorByIdAsync(int id);
    Task CreateAuthorAsync(AuthorForCreationDto author);
    Task UpdateAuthorAsync(int id, AuthorForUpdateDto author);
    Task DeleteAuthorAsync(int id);
    Task<int> CountAuthorsAsync(AuthorParameters requestParameters);
}
