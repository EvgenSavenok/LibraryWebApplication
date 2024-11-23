using Application.Contracts;
using Application.Contracts.ServicesContracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly ICountAuthorsUseCase _countAuthorsUseCase;
        private readonly ICreateAuthorUseCase _createAuthorUseCase;
        private readonly IDeleteAuthorUseCase _deleteAuthorUseCase;
        private readonly IGetAllAuthorsUseCase _getAllAuthorsUseCase;
        private readonly IGetAuthorByIdUseCase _getAuthorByIdUseCase;
        private readonly IUpdateAuthorUseCase _updateAuthorUseCase;

        public AuthorService(
            ICountAuthorsUseCase countAuthorsUseCase,
            ICreateAuthorUseCase createAuthorUseCase,
            IDeleteAuthorUseCase deleteAuthorUseCase,
            IGetAllAuthorsUseCase getAllAuthorsUseCase,
            IGetAuthorByIdUseCase getAuthorByIdUseCase,
            IUpdateAuthorUseCase updateAuthorUseCase)
        {
            _countAuthorsUseCase = countAuthorsUseCase;
            _createAuthorUseCase = createAuthorUseCase;
            _deleteAuthorUseCase = deleteAuthorUseCase;
            _getAllAuthorsUseCase = getAllAuthorsUseCase;
            _getAuthorByIdUseCase = getAuthorByIdUseCase;
            _updateAuthorUseCase = updateAuthorUseCase;
        }
        public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(AuthorParameters requestParameters)
            => await _getAllAuthorsUseCase.ExecuteAsync(requestParameters);

        public async Task<AuthorDto> GetAuthorByIdAsync(int id)
            => await _getAuthorByIdUseCase.ExecuteAsync(id);

        public async Task CreateAuthorAsync(AuthorForCreationDto author)
            => await _createAuthorUseCase.ExecuteAsync(author);

        public async Task UpdateAuthorAsync(int id, AuthorForUpdateDto author)
            => await _updateAuthorUseCase.ExecuteAsync(id, author);

        public async Task DeleteAuthorAsync(int id)
            => await _deleteAuthorUseCase.ExecuteAsync(id);
        
        public async Task<int> CountAuthorsAsync(AuthorParameters requestParameters)
            => await _countAuthorsUseCase.ExecuteAsync(requestParameters);
    }
}
