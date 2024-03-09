using Book_App.Data;
using Book_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Book_App.Controllers.Comments
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int bookId, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            var book = await _context.Books.Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                Content = content,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                CreatedAt = DateTime.UtcNow,
                BookId = bookId
            };

            book.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Books", new { id = bookId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Comment updatedComment)
        {
            if (id != updatedComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var comment = await _context.Comments.FindAsync(id);

                    if (comment == null || comment.UserId != User.Identity.Name)
                    {
                        return NotFound();
                    }

                    comment.Content = updatedComment.Content;

                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Details", "Books", new { id = updatedComment.BookId });
            }

            return View(updatedComment);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            // Redirect to the book details page
            return RedirectToAction("Details", "Books", new { id = comment.BookId });
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
