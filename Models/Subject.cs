using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EyeAttend.Models
{
    [Table("Subjects")]  // Explicitly set the table name in the database
    public class Subject
    {
        [Key]  // Marks the property as the primary key
        public int Id { get; set; }

        [Required]  // This field is required
        [StringLength(255)]  // Limit the length of the subject name to 255 characters
        public string SubjectName { get; set; }

        // Relationship with the Course entity
        [ForeignKey("CourseId")]  // Indicates the foreign key
        public int CourseId { get; set; } = 1;  // Optional default value if needed
        public Course Course { get; set; }  // Navigation property for the Course

        // Relationship with the User (Staff) entity
        [ForeignKey("Staff")]
        public int StaffId { get; set; }
        public Staff Staff { get; set; }

        // Timestamps (optional but recommended)
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property to establish a relationship with Attendance (if applicable)
        public ICollection<Attendance>? Attendances { get; set; }
    }
}
