namespace Book_App.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? BookId { get; set; } // Foreign key to Book entity
        public int? MovieId { get; set; } // Foreign key to Movie entity
        public int RatingValue { get; set; } // Numeric Scale 1 - 5
        public string Comment { get; set; }

        public User User { get; set; }
        public Book Book { get; set; }
        public Movie Movie { get; set; }
    }
}
