﻿using Book_App.Common;
using System.ComponentModel.DataAnnotations;

namespace Book_App.Models
{
    public class CreateMovieViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(Constants.MediaConstants.TitleMaxLength, MinimumLength = Constants.MediaConstants.TitleMinLength, ErrorMessage = "Title must be between 2 and 25 characters.")]
        public string Title { get; set; }

        [StringLength(Constants.MediaConstants.DescriptionMaxLength, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [StringLength(Constants.MediaConstants.DirectorNameMaxLength, ErrorMessage = "Author's name cannot exceed 20 characters.")]
        public string Director { get; set; }

        [StringLength(Constants.MediaConstants.ImgUrlMaxLength, ErrorMessage = "Image URL cannot exceed 2048 characters.")]
        public string ImgUrl { get; set; }

        [Range(Constants.MediaConstants.YearPublishedMinValue, Constants.MediaConstants.YearPublishedMaxValue, ErrorMessage = "Year published must be between 1000 and the current year.")]
        public int YearPublished { get; set; }

        [Range(Constants.MediaConstants.DurationMinLength, Constants.MediaConstants.DurationMaxLength, ErrorMessage = "Duration must be between 20 and 600 minutes.")]
        public int Duration { get; set; }
        public List<int> Genres { get; set; }
        public List<Genre> AllGenres { get; set; } = new List<Genre>();
    }
}
