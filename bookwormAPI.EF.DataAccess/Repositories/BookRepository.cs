using bookwormAPI.EF.DataAccess.Context;
using bookwormAPI.EF.DataAccess.DTO;
using bookwormAPI.EF.DataAccess.Models;
using bookwormAPI.EF.DataAccess.Repositories.Interfaces;
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

        Task<User> IBookRepository.CreateBook(BookDTO user)
        {
            throw new NotImplementedException();
        }

        Task IBookRepository.DeleteBook(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Book>> IBookRepository.GetAllBooks()
        {
            throw new NotImplementedException();
        }

        Task<User> IBookRepository.GetBookById(int id)
        {
            throw new NotImplementedException();
        }

        Task<User> IBookRepository.UpdateBook(int id, string bookTitle, string bookAuthor, int bookPages, string bookStatus, string bookFeeling)
        {
            throw new NotImplementedException();
        }
    }
}
