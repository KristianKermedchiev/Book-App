namespace Book_App.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int YearPublished { get; set; }
        public string OwnerId { get; set; } = null;
        public User Owner { get; set; } = null;
        public decimal Price { get; set; }
        public int Pages { get; set; }
    }
}
