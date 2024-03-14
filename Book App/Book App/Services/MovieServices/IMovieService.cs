using Book_App.Models;

namespace Book_App.Services.MovieServices
{
    public interface IMovieService
    {
        void CreateMovie(Movie movie);
        void UpdateMovie(Movie updatedMovie);
        Movie GetMovieById(int movieId);
        List<Movie> GetPendingSubmissions();
        void ApproveMovie(int movieId);
        void RejectMovie(int movieId);

    }
}
