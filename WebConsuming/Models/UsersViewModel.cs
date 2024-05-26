using System.ComponentModel.DataAnnotations;

namespace WebConsuming.Models
{
    public class UsersViewModel
    {
        public Guid Id { get; set; }
        
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
