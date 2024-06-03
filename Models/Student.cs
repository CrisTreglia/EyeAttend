using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EyeAttend.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [ForeignKey("Profile")]
        public int? ProfileID { get; set; }
        public Profile? Profile { get; set; }

        public string Gender { get; set; }

        [ForeignKey("SessionYear")] // Assuming the foreign key in Student is SessionYearId
        public int? SessionYearId { get; set; }
        public SessionYear? SessionYear { get; set; }

        [ForeignKey("Course")] // Assuming the foreign key in Student is SessionYearId
        public int? CourseId { get; set; }
        public Course? Course { get; set; }
        // For multiple profile pictures
        public ICollection<StudentPhoto>? StudentPhoto { get; set; } = new List<StudentPhoto>();

        // ... (You'll need a separate Course model and a relationship here)

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
