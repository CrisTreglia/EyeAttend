using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EyeAttend.Models
{
    [Table("Admins")] // Explicit table name for Admin
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminID { get; set; }

        // Assuming ProfileID is the foreign key from the Profile model
        [ForeignKey("Profile")]
        public int ProfileID { get; set; }
        public Profile Profile { get; set; } // Navigation property

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]  // Adjust if using hashing (hashes are longer)
        public string Password { get; set; }

        // ... any additional admin-specific fields
    }
}
