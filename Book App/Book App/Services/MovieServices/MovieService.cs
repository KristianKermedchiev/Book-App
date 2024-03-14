using Book_App.Data;
using Book_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_App.Services.MovieServices
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        public void UpdateMovie(Movie updatedMovie)
        {
            _context.Entry(updatedMovie).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Movie GetMovieById(int movieId)
        {
            return _context.Movies
                .Include(b => b.Genres)
                .FirstOrDefault(b => b.Id == movieId);
        }

        public List<Movie> GetPendingSubmissions()
        {
            var pendingSubmissions = _context.Movies
                .Where(movie => !movie.IsApproved)
                .Include(movie => movie.Genres)
                .ToList();

            return pendingSubmissions;
        }

        public void ApproveMovie(int movieId)
        {
            var movie = _context.Movies.Find(movieId);
            if (movie != null)
            {
                movie.IsApproved = true;
                _context.SaveChanges();
            }
        }

        public void RejectMovie(int movieId)
        {
            var movie = _context.Movies.Find(movieId);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
        }
    }
}
