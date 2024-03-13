using Book_App.Common;
using System.ComponentModel.DataAnnotations;

namespace Book_App.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(Constants.CommentConstants.ContentMaxLength, ErrorMessage = "Content cannot exceed 1000 characters.")]
        public string Content { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
