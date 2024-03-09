namespace Book_App.Models
{
    public class PaginatedBooksViewModel
    {
        public List<Book> Books { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
