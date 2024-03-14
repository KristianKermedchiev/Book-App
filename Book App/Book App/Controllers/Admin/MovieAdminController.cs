using Book_App.Services.MovieServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Administrator")]
public class MovieAdminController : Controller
{
    private readonly IMovieService _movieService;

    public MovieAdminController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public IActionResult MovieDashboard()
    {
        var pendingSubmissions = _movieService.GetPendingSubmissions();
        foreach (var movie in pendingSubmissions)
        {
            Console.WriteLine($"Movie {movie.Id} Genres: {string.Join(", ", movie.Genres.Select(genre => genre.Name))}");
        }
        return View(pendingSubmissions);
    }

    [HttpPost]
    public IActionResult Approve(int id)
    {
        _movieService.ApproveMovie(id);
        return RedirectToAction("MovieDashboard");
    }

    [HttpPost]
    public IActionResult Reject(int id)
    {
        _movieService.RejectMovie(id);
        return RedirectToAction("MovieDashboard");
    }
}
