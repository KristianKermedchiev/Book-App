using Book_App.Data;
using Book_App.Models;
using Book_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Book_App.Controllers.Books
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ApplicationDbContext _context;

        public BooksController(BookService bookService, ApplicationDbContext context)
        {
            _bookService = bookService;
            _context = context;
        }

        public IActionResult Create()
        {
            var viewModel = new CreateBookViewModel
            {
                AllGenres = _context.Genres.ToList()
            };

            return View("CreateBook", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newBook = new Book
                {
                    Title = model.Title,
                    Description = model.Description,
                    Author = model.Author,
                    ImgUrl = model.ImgUrl,
                    YearPublished = model.YearPublished,
                    Pages = model.Pages,
                    OwnerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                };
                foreach (var genreId in model.Genres)
                {
                    var genre = _context.Genres.Find(genreId);
                    if (genre != null)
                    {
                        newBook.Genres.Add(genre);
                    }
                }

                _bookService.CreateBook(newBook);

                return RedirectToAction("AllBooks");
            }

            model.AllGenres = _context.Genres.ToList();
            return View(model);
        }

        public IActionResult AllBooks()
        {
            return View();
        }
    }
}
