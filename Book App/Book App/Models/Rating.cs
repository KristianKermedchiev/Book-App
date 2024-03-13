namespace Book_App.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; } 
        public int RatingValue { get; set; } 

        public User User { get; set; }
        public Book Book { get; set; }
    }
}
