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
            return _context.Books
                .Include(b => b.Genres) 
                .FirstOrDefault(b => b.Id == bookId);
        }

        public List<Book> GetPendingSubmissions()
        {
            var pendingSubmissions = _context.Books
                .Where(book => !book.IsApproved)
                .Include(book => book.Genres)
                .ToList();

            return pendingSubmissions;
        }

        public void ApproveBook(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                book.IsApproved = true;
                _context.SaveChanges();
            }
        }

        public void RejectBook(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }
    }
}
