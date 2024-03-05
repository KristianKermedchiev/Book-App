using Microsoft.AspNetCore.Identity;
using static Book_App.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Book_App.Models
{
    public class User : IdentityUser
    {
        [StringLength(UserConstants.FirstNameMaxLength, MinimumLength = UserConstants.FirstNameMinLength, ErrorMessage = "First name must be between 2 and 25 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(UserConstants.LastNameMaxLength, MinimumLength = UserConstants.LastNameMinLength, ErrorMessage = "Last name must be between 2 and 25 characters.")]
        public string LastName { get; set; } = string.Empty;

    }
}
