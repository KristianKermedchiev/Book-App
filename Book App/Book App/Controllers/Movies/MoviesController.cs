using Book_App.Data;
using Book_App.Models;
using Book_App.Services.MovieServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Book_App.Controllers.Movies
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ApplicationDbContext _context;

        public MoviesController(MovieService movieService, ApplicationDbContext context)
        {
            _movieService = movieService;
            _context = context;
        }

        public IActionResult AllMovies(int page = 1, int pageSize = 3)
        {
            var approvedMovies = _context.Movies.Where(movie => movie.IsApproved);
            var totalMovies = approvedMovies.Count();

            var totalPages = (int)Math.Ceiling((double)totalMovies / pageSize);

            var movies = _context.Movies
                .Where(movie => movie.IsApproved)
                .Include(movie => movie.Genres)
                .OrderByDescending(movie => movie.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new PaginatedMoviesViewModel
            {
                Movies = movies,
                CurrentPage = page,
                TotalPages = totalPages,
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateMovieViewModel
            {
                AllGenres = _context.Genres.ToList()
            };

            return View("CreateMovie", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateMovieViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newMovie = new Movie
                {
                    Title = model.Title,
                    Description = model.Description,
                    Director = model.Director,
                    ImgUrl = model.ImgUrl,
                    YearPublished = model.YearPublished,
                    Duration = model.Duration,
                    OwnerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                };
                foreach (var genreId in model.Genres)
                {
                    var genre = _context.Genres.Find(genreId);
                    if (genre != null)
                    {
                        newMovie.Genres.Add(genre);
                    }
                }

                _movieService.CreateMovie(newMovie);

                return RedirectToAction("AllMovies");
            }

            model.AllGenres = _context.Genres.ToList();

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Comments)
                .ThenInclude(c => c.User)
                .Include(m => m.Genres)
                .Include(m => m.Ratings)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var ratings = _context.MovieRating.Where(r => r.MovieId == id);
            _context.MovieRating.RemoveRange(ratings);

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction("AllMovies");
        }

        [HttpPost]
        public async Task<IActionResult> Rate(int movieId, int ratingValue)
        {
            var movie = await _context.Movies.Include(m => m.Ratings).FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var existingRating = _context.MovieRating.FirstOrDefault(r => r.UserId == userId && r.MovieId == movieId);

            if (existingRating != null)
            {
                existingRating.RatingValue = ratingValue;
            }
            else
            {
                var newRating = new MovieRating
                {
                    UserId = userId,
                    MovieId = movieId,
                    RatingValue = ratingValue
                };

                _context.MovieRating.Add(newRating);
            }

            movie.AverageRating = movie.Ratings.Average(r => r.RatingValue);

            _context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var movie = _movieService.GetMovieById(id);

            if (movie == null)
            {
                return NotFound();
            }

            var viewModel = new UpdateMovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Director = movie.Director,
                ImgUrl = movie.ImgUrl,
                YearPublished = movie.YearPublished,
                Duration = movie.Duration,
                Genres = movie.Genres.Select(g => g.Id).ToList(),
                AllGenres = _context.Genres.ToList()
            };

            return View("UpdateMovie", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(UpdateMovieViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingMovie = _context.Movies
                    .Include(m => m.Genres)
                    .SingleOrDefault(m => m.Id == model.Id);

                if (existingMovie == null)
                {
                    return NotFound();
                }

                existingMovie.Title = model.Title;
                existingMovie.Description = model.Description;
                existingMovie.Director = model.Director;
                existingMovie.ImgUrl = model.ImgUrl;
                existingMovie.YearPublished = model.YearPublished;
                existingMovie.Duration = model.Duration;

                existingMovie.Genres.Clear();

                foreach (var genreId in model.Genres)
                {
                    var genre = _context.Genres.Find(genreId);
                    if (genre != null)
                    {
                        existingMovie.Genres.Add(genre);
                    }
                }

                _context.Update(existingMovie);
                _context.SaveChanges();

                return RedirectToAction("AllMovies");
            }

            model.AllGenres = _context.Genres.ToList();

            return View("UpdateMovies", model);
        }
    }
}
