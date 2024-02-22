using Microsoft.AspNetCore.Identity;

namespace Book_App.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public List<Book> WishListBooks { get; set; } = new List<Book>();
        public List<Book> OwnedBooks { get; set; } = new List<Book>();
    }
}
