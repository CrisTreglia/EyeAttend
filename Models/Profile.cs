using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EyeAttend.Models
{
    [Table("Profiles")] // Explicitly sets the table name in the database
    public class Profile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Don't auto-increment
        public int ProfileID { get; set; }

        [Required]
        [StringLength(10)] // Adjust if label gets longer
        public string ProfileName { get; set; }

        // ... any other profile-specific fields (e.g., contact info)
    }
}
