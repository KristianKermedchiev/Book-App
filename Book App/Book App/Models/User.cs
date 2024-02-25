using Microsoft.AspNetCore.Identity;
using static Book_App.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Book_App.Models
{
    public class User : IdentityUser
    {
        [StringLength(UserConstants.FirstNameMaxLength, MinimumLength = UserConstants.FirstNameMinLength, ErrorMessage = "First name must be between 2 and 25 characters.")]
        public string FirstName { get; set; }

        [StringLength(UserConstants.LastNameMaxLength, MinimumLength = UserConstants.LastNameMinLength, ErrorMessage = "Last name must be between 2 and 25 characters.")]
        public string LastName { get; set; }

        [StringLength(UserConstants.EmailMaxLength, MinimumLength = UserConstants.EmailMinLength, ErrorMessage = "Email must be between 7 and 35 characters.")]
        public string EmailAddress { get; set; }

        public List<Book> WishListBooks { get; set; } = new List<Book>();
        public List<Book> OwnedBooks { get; set; } = new List<Book>();
    }
}
