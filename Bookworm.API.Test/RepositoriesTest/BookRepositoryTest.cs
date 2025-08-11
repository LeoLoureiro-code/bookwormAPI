using bookwormAPI.EF.DataAccess.Context;
using bookwormAPI.EF.DataAccess.Models;
using bookwormAPI.EF.DataAccess.Repositories;
using bookwormAPI.EF.DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Collections.Generic;

public class BookRepositoryTests
{
    private BookwormContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<BookwormContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .EnableSensitiveDataLogging()
            .Options;

        return new BookwormContext(options);
    }

    [Fact]
    public async Task CreateBook_ShouldAddBook()
    {
        using var context = GetDbContext();
        var repo = new BookRepository(context);

        var bookDto = new BookDTO
        {
            Title = "Test Book",
            Author = "Author Name",
            Pages = 100,
            Status = "Read",
            Feeling = "Excited",
        };

        var createdBook = await repo.CreateBook(bookDto);

        Assert.NotNull(createdBook);
        Assert.Equal("Test Book", createdBook.BookTitle);
        Assert.Equal("Author Name", createdBook.BookAuthor);
        Assert.Equal("Read", createdBook.BookStatus);
        Assert.Equal(100, createdBook.BookPages);
        Assert.Equal("Excited", createdBook.BookFeeling);
    }

    [Fact]
    public async Task GetBookById_ShouldReturnBook_WhenExists()
    {
        using var context = GetDbContext();
        var repo = new BookRepository(context);

        var book = new Book
        {
            BookTitle = "Sample",
            BookAuthor = "Author",
            BookPages = 123,
            BookStatus = "to buy",
            BookFeeling = "Happy"
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();

        var result = await repo.GetBookById(book.BookId);

        Assert.NotNull(result);
        Assert.Equal("Sample", result.BookTitle);
    }

    [Fact]
    public async Task GetBookById_ShouldThrowException_WhenNotFound()
    {
        using var context = GetDbContext();
        var repo = new BookRepository(context);

        await Assert.ThrowsAsync<Exception>(() => repo.GetBookById(999));
    }

   
    [Fact]
    public async Task UpdateBook_ShouldModifyBook()
    {
        using var context = GetDbContext();
        var repo = new BookRepository(context);

        var book = new Book
        {
            BookTitle = "Old Title",
            BookAuthor = "Old Author",
            BookPages = 200,
            BookFeeling = "Neutral",
            BookStatus = "Unread"
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();

        var updatedBook = await repo.UpdateBook(
            book.BookId,
            "New Title",
            "New Author",
            300,
            "Read",
            "Excited");

        Assert.Equal("New Title", updatedBook.BookTitle);
        Assert.Equal("New Author", updatedBook.BookAuthor);
        Assert.Equal(300, updatedBook.BookPages);
        Assert.Equal("Read", updatedBook.BookStatus);
        Assert.Equal("Excited", updatedBook.BookFeeling);
    }

    [Fact]
    public async Task UpdateBook_ShouldThrowException_WhenBookNotFound()
    {
        using var context = GetDbContext();
        var repo = new BookRepository(context);

        await Assert.ThrowsAsync<Exception>(() =>
            repo.UpdateBook(999, "Title", "Author", 100, "Status", "Feeling"));
    }

    [Fact]
    public async Task DeleteBook_ShouldRemoveBook()
    {
        using var context = GetDbContext();
        var repo = new BookRepository(context);

        var book = new Book
        {
            BookTitle = "To be deleted",
            BookAuthor = "Author",
            BookPages = 150,
            BookStatus = "read",
            BookFeeling = "Sad"
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();

        await repo.DeleteBook(book.BookId);

        var deletedBook = await context.Books.FindAsync(book.BookId);
        Assert.Null(deletedBook);
    }

    [Fact]
    public async Task DeleteBook_ShouldThrowException_WhenBookNotFound()
    {
        using var context = GetDbContext();
        var repo = new BookRepository(context);
        await Assert.ThrowsAsync<Exception>(() => repo.DeleteBook(999));
    }
}
