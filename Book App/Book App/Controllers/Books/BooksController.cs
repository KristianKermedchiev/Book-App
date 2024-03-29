﻿using Book_App.Data;
using Book_App.Models;
using Book_App.Services.BookServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public IActionResult Update(int id)
        {
            var book = _bookService.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new UpdateBookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
                ImgUrl = book.ImgUrl,
                YearPublished = book.YearPublished,
                Pages = book.Pages,
                Genres = book.Genres.Select(g => g.Id).ToList(),
                AllGenres = _context.Genres.ToList()
            };

            return View("UpdateBook", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(UpdateBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingBook = _context.Books
                    .Include(b => b.Genres)
                    .SingleOrDefault(b => b.Id == model.Id);

                if (existingBook == null)
                {
                    return NotFound();
                }

                existingBook.Title = model.Title;
                existingBook.Description = model.Description;
                existingBook.Author = model.Author;
                existingBook.ImgUrl = model.ImgUrl;
                existingBook.YearPublished = model.YearPublished;
                existingBook.Pages = model.Pages;

                existingBook.Genres.Clear();

                foreach (var genreId in model.Genres)
                {
                    var genre = _context.Genres.Find(genreId);
                    if (genre != null)
                    {
                        existingBook.Genres.Add(genre);
                    }
                }

                _context.Update(existingBook);
                _context.SaveChanges();

                return RedirectToAction("AllBooks");
            }

            model.AllGenres = _context.Genres.ToList();
            return View("UpdateBook", model);
        }


        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.Comments)
                .ThenInclude(c => c.User)
                .Include(b => b.Genres)
                .Include(b => b.Ratings)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var ratings = _context.Ratings.Where(r => r.BookId == id);
            _context.Ratings.RemoveRange(ratings);

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return RedirectToAction("AllBooks");
        }

        public IActionResult AllBooks(int page = 1, int pageSize = 3)
        {
            var approvedBooks = _context.Books.Where(book => book.IsApproved);
            var totalBooks = approvedBooks.Count();

            var totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

            var books = _context.Books
                .Where(book => book.IsApproved)
                .Include(book => book.Genres)
                .OrderByDescending(book => book.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new PaginatedBooksViewModel
            {
                Books = books,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Rate(int bookId, int ratingValue)
        {
            var book = await _context.Books.Include(b => b.Ratings).FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var existingRating = _context.Ratings.FirstOrDefault(r => r.UserId == userId && r.BookId == bookId);

            if (existingRating != null)
            {
                existingRating.RatingValue = ratingValue;
            }
            else
            {
                var newRating = new BookRating
                {
                    UserId = userId,
                    BookId = bookId,
                    RatingValue = ratingValue
                };

                _context.Ratings.Add(newRating);
            }

            book.AverageRating = book.Ratings.Average(r => r.RatingValue);

            _context.SaveChanges();

            return Ok();
        }
    }
}
