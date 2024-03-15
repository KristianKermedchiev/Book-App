namespace Book_App.Models
{
    public class MovieRating
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public int RatingValue { get; set; }

        public User User { get; set; }
        public Movie Movie { get; set; }
    }
}
