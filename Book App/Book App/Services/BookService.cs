using Book_App.Data;
using Book_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_App.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(Book updatedBook)
        {
            _context.Entry(updatedBook).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Book GetBookById(int bookId)
        {
            // Retrieve the book by ID from the database
            return _context.Books
                .Include(b => b.Genres) // Include related entities if needed
                .FirstOrDefault(b => b.Id == bookId);
        }
    }
}
