using bookwormAPI.DTO;
using bookwormAPI.EF.DataAccess.Context;
using bookwormAPI.EF.DataAccess.DTO;
using bookwormAPI.EF.DataAccess.Models;
using bookwormAPI.EF.DataAccess.Repositories.Interfaces;
using bookwormAPI.EF.DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookwormAPI.EF.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {

        private readonly BookwormContext _context;


        public BookRepository(BookwormContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();

        }

        public async Task<Book> GetBookById(int id)
        {
            Book? book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                throw new Exception("Book not found");
            }

            return book;
        }

        public async Task<IEnumerable<Book>> GetBooksByUserIdAsync(int userId)
        {
            return await _context.Books
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<Book> CreateBook(BookDTO book)
        {
            var bookEntity = new Book
            {
                BookTitle = book.Title,
                BookAuthor = book.Author,
                BookPages = book.Pages,
                BookFeeling = book.Feeling,
            };

            await _context.Books.AddAsync(bookEntity);
            await _context.SaveChangesAsync();
            return bookEntity;
        }

        public async Task<Book> UpdateBook(int id, string bookTitle, string bookAuthor, int bookPages, string bookStatus, string bookFeeling)
        {
            Book? existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
            {
                throw new Exception("Book not found");
            }

            existingBook.BookTitle = bookTitle;
            existingBook.BookAuthor = bookAuthor;
            existingBook.BookPages = bookPages;
            existingBook.BookStatus = bookStatus;
            existingBook.BookFeeling = bookFeeling;

            await _context.SaveChangesAsync();
            return existingBook;
        }

        public async Task DeleteBook(int id)
        {
            Book? existingBook = await _context.Books.FindAsync(id);
            
            if(existingBook == null)
            {
                throw new Exception("Book not found");
            }

            _context.Remove(existingBook);
            await _context.SaveChangesAsync();
        }

     
    }
}
