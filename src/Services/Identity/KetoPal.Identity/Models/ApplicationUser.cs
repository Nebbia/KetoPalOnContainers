using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KetoPal.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
