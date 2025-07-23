using bookwormAPI.DTO;
using bookwormAPI.EF.DataAccess.DTO;
using bookwormAPI.EF.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookwormAPI.EF.DataAccess.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks();

        Task<Book> GetBookById(int id);

        Task<Book> CreateBook(BookDTO user);

        Task<Book> UpdateBook(int id, string bookTitle, string bookAuthor, int bookPages, string bookStatus, string bookFeeling);

        Task DeleteBook(int id);
    }
}
