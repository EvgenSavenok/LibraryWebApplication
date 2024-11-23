using Domain.Contracts;

namespace Application.Contracts;

public interface IRepositoryManager
{
    IBookRepository Book { get; }
    IAuthorRepository Author { get; }
    IUserBookBorrowRepository Borrow { get; }
    Task SaveAsync();
}
