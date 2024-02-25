using Book_App.Common;
using System.ComponentModel.DataAnnotations;

namespace Book_App.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(Constants.MediaConstants.TitleMaxLength, MinimumLength = Constants.MediaConstants.TitleMinLength, ErrorMessage = "Title must be between 2 and 25 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(Constants.MediaConstants.DescriptionMaxLength, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [StringLength(Constants.MediaConstants.DirectorNameMaxLength, ErrorMessage = "Author's name cannot exceed 10 characters.")]
        public string Author { get; set; } = string.Empty;

        [StringLength(Constants.MediaConstants.ImgUrlMaxLength, ErrorMessage = "Image URL cannot exceed 2048 characters.")]
        public string ImgUrl { get; set; }

        [Range(Constants.MediaConstants.YearPublishedMinValue, Constants.MediaConstants.YearPublishedMaxValue, ErrorMessage = "Year published must be a valid one.")]
        public int YearPublished { get; set; }

        public ICollection<Genre> Genres { get; set; } = new List<Genre>();

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public int Duration { get; set; }
        public string OwnerId { get; set; } = null;
        public User Owner { get; set; } = null;
    }
}
