namespace Book_App.Models
{
    public class PaginatedMoviesViewModel
    {
        public List<Movie> Movies { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
