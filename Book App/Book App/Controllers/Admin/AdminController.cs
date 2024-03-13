using Book_App.Services.BookServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
    private readonly IBookService _bookService;

    public AdminController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public IActionResult Dashboard()
    {
        var pendingSubmissions = _bookService.GetPendingSubmissions();
        foreach (var book in pendingSubmissions)
        {
            Console.WriteLine($"Book {book.Id} Genres: {string.Join(", ", book.Genres.Select(genre => genre.Name))}");
        }
        return View(pendingSubmissions);
    }

    [HttpPost]
    public IActionResult Approve(int id)
    {
        _bookService.ApproveBook(id);
        return RedirectToAction("Dashboard");
    }

    [HttpPost]
    public IActionResult Reject(int id)
    {
        _bookService.RejectBook(id);
        return RedirectToAction("Dashboard");
    }
}
