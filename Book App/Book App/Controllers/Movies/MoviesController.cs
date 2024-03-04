using Microsoft.AspNetCore.Mvc;

namespace Book_App.Controllers.Movies
{
    public class MoviesController : Controller
    {
        public IActionResult AllMovies()
        {
            return View();
        }
    }
}
