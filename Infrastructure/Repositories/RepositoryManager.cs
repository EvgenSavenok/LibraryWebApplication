using Application.Contracts;
using Domain.Contracts;
using Repository;

namespace Infrastructure.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private ApplicationContext _repositoryContext;
    private IAuthorRepository _authorRepository;
    private IUserBookBorrowRepository _borrowRepository;
    private IBookRepository _bookRepository;
    public RepositoryManager(ApplicationContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }
    public IBookRepository Book
    {
        get
        {
            if(_bookRepository == null)
                _bookRepository = new BookRepository(_repositoryContext);
            return _bookRepository;
        }
    }
    
    public IAuthorRepository Author
    {
        get
        {
            if(_authorRepository == null)
                _authorRepository = new AuthorRepository(_repositoryContext);
            return _authorRepository;
        }
    }

    public IUserBookBorrowRepository Borrow
    {
        get
        {
            if (_borrowRepository == null)
                _borrowRepository = new UserBookBorrowRepository(_repositoryContext);
            return _borrowRepository;
        }
    }
    
    public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
}
