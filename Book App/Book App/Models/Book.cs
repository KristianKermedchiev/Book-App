using System.ComponentModel.DataAnnotations;
using Book_App.Common;

namespace Book_App.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(Constants.BookConstatns.TitleMaxLength, MinimumLength = Constants.BookConstatns.TitleMinLength, ErrorMessage = "Title must be between 5 and 25 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(Constants.BookConstatns.DescriptionMaxLength, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [StringLength(Constants.BookConstatns.AuthorNameMaxLength, ErrorMessage = "Author's name cannot exceed 10 characters.")]
        public string Author { get; set; } = string.Empty;

        [StringLength(Constants.BookConstatns.ImgUrlMaxLength, ErrorMessage = "Image URL cannot exceed 2048 characters.")]
        public string ImgUrl { get; set; }

        [StringLength(Constants.BookConstatns.GenerMaxLength, ErrorMessage = "Genre cannot exceed 15 characters.")]
        public string Genre { get; set; } = string.Empty;

        [Range(Constants.BookConstatns.YearPublishedMinValue, Constants.BookConstatns.YearPublishedMaxValue, ErrorMessage = "Year published must be between 1000 and the current year.")]
        public int YearPublished { get; set; }

        [Range(Constants.BookConstatns.PriceMinValue, Constants.BookConstatns.PriceMaxValue, ErrorMessage = "Price must be between 1 and 99.99.")]
        public double Price { get; set; }

        [Range(Constants.BookConstatns.PagesMinLength, Constants.BookConstatns.PagesMaxLength, ErrorMessage = "Number of pages must be between 50 and 1999.")]
        public int Pages { get; set; }

        public string OwnerId { get; set; } = null;

        public User Owner { get; set; } = null;
    }
}
