using bookwormAPI.DTO;
using bookwormAPI.EF.DataAccess.DTO;
using bookwormAPI.EF.DataAccess.Models;
using bookwormAPI.EF.DataAccess.Repositories;
using bookwormAPI.EF.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bookwormAPI.Controllers
{
    [Route("Bookworm/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookRepository _bookRepository;

        public BookController(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        //Get: api/all-books
        [HttpGet("all-book")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            try
            {
                var users = await _bookRepository.GetAllBooks();
                if (users == null || !users.Any())
                {
                    return NotFound("No users Found");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "An error occurred while fetching books.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        //Get: api/all-book/User
        [HttpGet("my-books")]
        public async Task<ActionResult<IEnumerable<Book>>> GetMyBooks()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var books = await _bookRepository.GetBooksByUserIdAsync(userId);

            if (books == null || !books.Any())
                return NotFound("No books found for your account.");

            return Ok(books);
        }

        //Get:api/Books/5

        [HttpGet("find-by-id/{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var user = await _bookRepository.GetBookById(id);

                if (user == null)
                {
                    return NotFound(
                        new
                        {
                            Message = $"Book with ID {id} was not found."

                        });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "An error occurred while fetching book.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        //POST: api/Book
        [HttpPost("create-book")]
        public async Task<ActionResult> CreateBook([FromBody] BookDTO book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "All fields are required."
                    });
                }
                var createdBook = await _bookRepository.CreateBook(book);

                return CreatedAtAction(
                nameof(GetBook),
                new { id = createdBook.BookId },
                createdBook
        );
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "An error occurred while creating the book.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        ////PUT: api/books/5
        ///DO IT AFTER THE JWT ARE CORRECT
        //[HttpPut("update-book/{id}")]
        //public async Task<ActionResult> UpdateBook(int id, [FromBody] Book book)
        //{
        //    try
        //    {
        //        // Check if route ID matches body ID (if applicable)
        //        if (book.BookId != 0 && book.BookId != id)
        //        {
        //            return BadRequest("Book ID in the body does not match URL.");
        //        }


        //        var updated = await _bookRepository.UpdateBook(id, book.BookTitle, book.BookAuthor, book.BookPages, book.BookStatus, book.BookFeeling);

        //        if (updated == null)
        //        {
        //            return NotFound($"Book with ID {id} not found.");
        //        }

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Problem(
        //            detail: ex.Message,
        //            title: "An error occurred while updating the book.",
        //            statusCode: StatusCodes.Status500InternalServerError);
        //    }
        //}
    }
}
