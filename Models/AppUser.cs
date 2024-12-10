using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ONIK_BANK.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }
    }
}
