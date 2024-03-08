using Book_App.Data;
using Book_App.Models;
using Book_App.Services;
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
