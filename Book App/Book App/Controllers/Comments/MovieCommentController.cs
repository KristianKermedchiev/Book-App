using Book_App.Data;
using Book_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Book_App.Controllers.Comments
{
    public class MovieCommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MovieCommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int movieId, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return RedirectToAction("Details", "Movies", new { id = movieId });
            }

            var movie = await _context.Movies.Include(m => m.Comments).FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return NotFound();
            }

            var comment = new MovieCommentModel
            {
                Content = content,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                CreatedAt = DateTime.UtcNow,
                MovieId = movieId
            };

            movie.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.MovieCommentModel.FindAsync(id);

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
            var comment = await _context.MovieCommentModel.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            _context.MovieCommentModel.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Movies", new {id = comment.MovieId});
    }

        public IActionResult Index()
        {
            return View();
        }
    }
}
