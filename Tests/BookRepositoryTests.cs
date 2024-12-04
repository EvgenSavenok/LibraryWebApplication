using Domain.Entities.Models;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Tests;

public class BookRepositoryTests
{
    private DbContextOptions<ApplicationContext> _dbContextOptions;

    public BookRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "TestLibraryDb")
            .Options;
    }
    
    [Fact]
    public async Task GetBookAsync_ShouldReturnBook_WhenBookExists()
    {
        var bookId = 1;
        var book = new Book
        {
            Id = bookId,
            BookTitle = "Test Book",
            ISBN = "1234567890123",
            Genre = BookGenre.Horrors,
            Description = "Test Description",
            Amount = 5,
            Author = new Author
            {
                Id = 1,
                Name = "Jon",
                LastName = "Smith",
                BirthDate = "1990-01-01",
                Country = "USA"
            },
            AuthorId = 1
        };

        await using (var context = new ApplicationContext(_dbContextOptions))
        {
            context.Books.Add(book);
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationContext(_dbContextOptions))
        {
            var repository = new BookRepository(context);

            var result = await repository.GetBookAsync(bookId, trackChanges: false);

            result.Should().NotBeNull();
            result.Id.Should().Be(bookId);
            result.BookTitle.Should().Be("Test Book");
            result.ISBN.Should().Be("1234567890123");
            result.Genre.Should().Be(BookGenre.Horrors);
            result.Description.Should().Be("Test Description");
            result.Amount.Should().Be(5);
        }
    }

    [Fact]
    public async Task GetBookAsync_ShouldReturnNull_WhenBookDoesNotExist()
    {
        var bookId = 999; 

        await using (var context = new ApplicationContext(_dbContextOptions))
        {
            var repository = new BookRepository(context);

            var result = await repository.GetBookAsync(bookId, trackChanges: false);

            result.Should().BeNull();
        }
    }
}
