using Microsoft.AspNetCore.Mvc;

namespace Book_App.Controllers.Books
{
    public class BooksController : Controller
    {
        public IActionResult AllBooks()
        {
            return View();
        }
    }
}
