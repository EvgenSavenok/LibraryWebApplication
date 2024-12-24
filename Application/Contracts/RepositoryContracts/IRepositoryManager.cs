namespace Application.Contracts.RepositoryContracts;

public interface IRepositoryManager
{
    IBookRepository Book { get; }
    IAuthorRepository Author { get; }
    IUserBookBorrowRepository Borrow { get; }
    Task SaveAsync();
}
