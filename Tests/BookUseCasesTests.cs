using Application.Contracts;
using Application.DataTransferObjects;
using Application.UseCases.BookUseCases;
using AutoMapper;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;
using FluentAssertions;
using FluentValidation;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repository;

namespace Tests;

public class BookUseCaseTests
{
    private DbContextOptions<ApplicationContext> _dbContextOptions;

    public BookUseCaseTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "TestLibraryDb")
            .Options;
    }

    [Fact]
    public async Task GetBookByIdAsync_ShouldReturnBook_WhenBookExists()
    {
        var book = new Book
        {
            Id = 1,
            BookTitle = "Test Book",
            ISBN = "1111111111111",
            Genre = BookGenre.Horrors,
            Description = "Horror tale",
            Amount = 3,
            Author = new Author
            {
                Id = 1,
                Name = "Jon",
                LastName = "Smith",
                BirthDate = "999.99.99",
                Country = "USA"
            },
            AuthorId = 1
        };
        
        var bookDto = new BookDto
        {
            Id = 1,
            BookTitle = "Test Book",
            ISBN = "1111111111111",
            Genre = BookGenre.Horrors,
            Description = "Horror tale",
            Amount = 3,
            AuthorId = 1,
            AuthorName = "Jon",
            AuthorLastName = "Smith"
        };

        await using (var context = new ApplicationContext(_dbContextOptions))
        {
            context.Books.Add(book);
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationContext(_dbContextOptions))
        {
            var repositoryManager = new RepositoryManager(context);
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILoggerManager>();

            mapperMock.Setup(mapper => mapper.Map<BookDto>(It.IsAny<Book>()))
                .Returns((Book src) => new BookDto
                {
                    Id = src.Id,
                    BookTitle = src.BookTitle,
                    ISBN = src.ISBN,
                    Genre = src.Genre,
                    Description = src.Description,
                    Amount = src.Amount,
                    AuthorId = src.AuthorId,
                    AuthorName = src.Author?.Name,
                    AuthorLastName = src.Author?.LastName
                });

            var useCase = new GetBookByIdUseCase(repositoryManager, mapperMock.Object, loggerMock.Object);

            var result = await useCase.ExecuteAsync(1);

            result.Should().NotBeNull();
            result.BookTitle.Should().Be("Test Book");
            result.ISBN.Should().Be("1111111111111");
            result.Genre.Should().Be(BookGenre.Horrors);
            result.Description.Should().Be("Horror tale");
            result.Amount.Should().Be(3);
            result.AuthorId.Should().Be(1);
        }
    }

    [Fact]
    public async Task CountBooksUseCase_ShouldReturnCorrectCount_WhenBooksMatchParameters()
    {
        var book1 = new Book
        {
            Id = 1,
            BookTitle = "Horror Story",
            ISBN = "1111111111111",
            Genre = BookGenre.Horrors,
            Description = "A scary book",
            Amount = 5,
            AuthorId = 1
        };

        var book2 = new Book
        {
            Id = 2,
            BookTitle = "Another Horror",
            ISBN = "1111111111112",
            Genre = BookGenre.Horrors,
            Description = "Another scary book",
            Amount = 3,
            AuthorId = 1
        };

        var bookParameters = new BookParameters
        {
            Genre = BookGenre.Horrors
        };

        await using (var context = new ApplicationContext(_dbContextOptions))
        {
            context.Books.RemoveRange(context.Books);
            await context.SaveChangesAsync();
            
            context.Books.AddRange(book1, book2);
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationContext(_dbContextOptions))
        {
            var repositoryManager = new RepositoryManager(context);
            var loggerMock = new Mock<ILoggerManager>();

            var useCase = new CountBooksUseCase(repositoryManager, loggerMock.Object);

            var result = await useCase.ExecuteAsync(bookParameters);

            result.Should().Be(2);
        }
    }
    
    [Fact]
    public async Task CreateBookUseCase_ShouldCreateBook_WhenBookIsValid()
    {
        var bookDto = new BookForCreationDto
        {
            BookTitle = "Test Book",
            ISBN = "1234567890123",
            Genre = BookGenre.Horrors,
            Description = "A scary book",
            Amount = 5,
            AuthorId = 1
        };

        var bookEntity = new Book
        {
            Id = 1,
            BookTitle = "Test Book",
            ISBN = "1234567890123",
            Genre = BookGenre.Horrors,
            Description = "A scary book",
            Amount = 5,
            AuthorId = 1
        };

        var repositoryMock = new Mock<IRepositoryManager>();
        var mapperMock = new Mock<IMapper>();
        var validatorMock = new Mock<IValidator<Book>>();
        var loggerMock = new Mock<ILoggerManager>();

        mapperMock.Setup(mapper => mapper.Map<Book>(bookDto))
            .Returns(bookEntity);

        validatorMock.Setup(validator => validator.ValidateAsync(bookEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        repositoryMock.Setup(repo => repo.Book.GetBookByIsbnAsync(bookEntity.ISBN))
            .ReturnsAsync((Book)null);

        repositoryMock.Setup(repo => repo.Book.Create(bookEntity));
        repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

        var useCase = new CreateBookUseCase(
            repositoryMock.Object, 
            mapperMock.Object, 
            validatorMock.Object, 
            loggerMock.Object
        );

        await useCase.ExecuteAsync(bookDto);

        repositoryMock.Verify(repo => repo.Book.Create(bookEntity), Times.Once);
        repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        loggerMock.Verify(logger => logger.LogInfo(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task DeleteBookUseCase_ShouldDeleteBook_WhenBookExists()
    {
        var bookId = 1;
        var bookEntity = new Book
        {
            Id = bookId,
            BookTitle = "Test Book",
            ISBN = "1234567890123",
            Genre = BookGenre.Horrors,
            Description = "A scary book",
            Amount = 5,
            AuthorId = 1
        };

        var repositoryMock = new Mock<IRepositoryManager>();
        var loggerMock = new Mock<ILoggerManager>();

        repositoryMock.Setup(repo => repo.Book.GetBookAsync(bookId, false))
            .ReturnsAsync(bookEntity);

        repositoryMock.Setup(repo => repo.Book.Delete(bookEntity));
        repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

        var useCase = new DeleteBookUseCase(repositoryMock.Object, loggerMock.Object);

        await useCase.ExecuteAsync(bookId);

        repositoryMock.Verify(repo => repo.Book.GetBookAsync(bookId, false), Times.Once);
        repositoryMock.Verify(repo => repo.Book.Delete(bookEntity), Times.Once);
        repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        loggerMock.Verify(logger => logger.LogInfo(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetBooksUseCase_ShouldReturnPagedResult_WhenBooksExist()
    {
        var bookParameters = new BookParameters
        {
            PageNumber = 1,
            PageSize = 5
        };

        var books = new List<Book>
        {
            new Book
            {
                Id = 1, BookTitle = "Book 1", ISBN = "1111111111111", Genre = BookGenre.Adventures,
                Description = "Adventure book", Amount = 3
            },
            new Book
            {
                Id = 2, BookTitle = "Book 2", ISBN = "2222222222222", Genre = BookGenre.Horrors,
                Description = "Horror book", Amount = 5
            }
        };

        var booksDto = books.Select(b => new BookDto
        {
            Id = b.Id,
            BookTitle = b.BookTitle,
            ISBN = b.ISBN,
            Genre = b.Genre,
            Description = b.Description,
            Amount = b.Amount
        });

        var repositoryMock = new Mock<IRepositoryManager>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILoggerManager>();

        repositoryMock.Setup(repo => repo.Book.GetAllBooksAsync(bookParameters, false))
            .ReturnsAsync(books);

        repositoryMock.Setup(repo => repo.Book.CountBooksAsync(bookParameters))
            .ReturnsAsync(books.Count);

        mapperMock.Setup(mapper => mapper.Map<IEnumerable<BookDto>>(books))
            .Returns(booksDto);

        var useCase = new GetBooksUseCase(repositoryMock.Object, mapperMock.Object, loggerMock.Object);

        var result = await useCase.ExecuteAsync(bookParameters);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(books.Count);
        result.TotalCount.Should().Be(books.Count);
        result.TotalPages.Should().Be(1); 
    }
    
    [Fact]
    public async Task UpdateBookUseCase_ShouldUpdateBook_WhenBookExistsAndIsValid()
    {
        var bookId = 1;
        var bookForUpdateDto = new BookForUpdateDto
        {
            BookTitle = "Updated Book",
            ISBN = "2222222222222",
            Genre = BookGenre.Adventures,
            Description = "Updated description",
            Amount = 5
        };

        var existingBook = new Book
        {
            Id = bookId,
            BookTitle = "Original Book",
            ISBN = "1111111111111",
            Genre = BookGenre.Horrors,
            Description = "Original description",
            Amount = 3
        };

        var repositoryMock = new Mock<IRepositoryManager>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILoggerManager>();
        var validatorMock = new Mock<IValidator<Book>>();

        repositoryMock.Setup(repo => repo.Book.GetBookAsync(bookId, true))
            .ReturnsAsync(existingBook);

        repositoryMock.Setup(repo => repo.Book.GetBookByIsbnAsync(bookForUpdateDto.ISBN))
            .ReturnsAsync((Book)null);

        mapperMock.Setup(mapper => mapper.Map(bookForUpdateDto, existingBook))
            .Callback(() =>
            {
                existingBook.BookTitle = bookForUpdateDto.BookTitle;
                existingBook.ISBN = bookForUpdateDto.ISBN;
                existingBook.Genre = bookForUpdateDto.Genre;
                existingBook.Description = bookForUpdateDto.Description;
                existingBook.Amount = bookForUpdateDto.Amount;
            });

        validatorMock.Setup(validator => validator.ValidateAsync(existingBook, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var useCase = new UpdateBookUseCase(
            repositoryMock.Object,
            mapperMock.Object,
            loggerMock.Object,
            validatorMock.Object);

        await useCase.ExecuteAsync(bookId, bookForUpdateDto);

        repositoryMock.Verify(repo => repo.Book.GetBookAsync(bookId, true), Times.Once);
        repositoryMock.Verify(repo => repo.Book.GetBookByIsbnAsync(bookForUpdateDto.ISBN), Times.Once);
        repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        mapperMock.Verify(mapper => mapper.Map(bookForUpdateDto, existingBook), Times.Once);
        validatorMock.Verify(validator => validator.ValidateAsync(existingBook, default), Times.Once);

        existingBook.BookTitle.Should().Be(bookForUpdateDto.BookTitle);
        existingBook.ISBN.Should().Be(bookForUpdateDto.ISBN);
        existingBook.Genre.Should().Be(bookForUpdateDto.Genre);
        existingBook.Description.Should().Be(bookForUpdateDto.Description);
        existingBook.Amount.Should().Be(bookForUpdateDto.Amount);
    }
}
