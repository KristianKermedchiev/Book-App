using Book_App.Models;

namespace Book_App.Services
{

    public interface IBookService
    {
        void CreateBook(Book book);
        Book GetBookById(int bookId);

        void UpdateBook(Book updatedBook);

        //IEnumerable<Book> GetAllBooks();
    }
}
