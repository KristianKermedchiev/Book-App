﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Book_App.Common;

namespace Book_App.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(Constants.MediaConstants.TitleMaxLength, MinimumLength = Constants.MediaConstants.TitleMinLength, ErrorMessage = "Title must be between 2 and 25 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(Constants.MediaConstants.DescriptionMaxLength, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [StringLength(Constants.MediaConstants.AuthorNameMaxLength, ErrorMessage = "Author's name cannot exceed 20 characters.")]
        public string Author { get; set; } = string.Empty;

        [StringLength(Constants.MediaConstants.ImgUrlMaxLength, ErrorMessage = "Image URL cannot exceed 2048 characters.")]
        public string ImgUrl { get; set; }

        [Range(Constants.MediaConstants.YearPublishedMinValue, Constants.MediaConstants.YearPublishedMaxValue, ErrorMessage = "Year published must be between 1000 and the current year.")]
        public int YearPublished { get; set; }

        [Range(Constants.MediaConstants.PagesMinLength, Constants.MediaConstants.PagesMaxLength, ErrorMessage = "Number of pages must be between 50 and 1999.")]
        public int Pages { get; set; }

        public ICollection<Genre> Genres { get; set; } = new List<Genre>();

        public ICollection<BookRating> Ratings { get; set; } = new List<BookRating>();

        public double AverageRating { get; set; }

        public string OwnerId { get; set; } = null;

        [NotMapped]
        public List<int> UpdatedGenreIds { get; set; }

        public bool IsApproved { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
