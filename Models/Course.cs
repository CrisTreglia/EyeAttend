using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EyeAttend.Models
{
    [Table("Courses")] // Explicitly sets the table name in the database
    public class Course
    {
        [Key]  // Marks the property as the primary key
        public int Id { get; set; }

        [Required] // Specifies that this field is required
        [StringLength(255)] // Equivalent to Django's max_length
        public string CourseName { get; set; }

        // Optionally add timestamps for tracking when the record was created and updated
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property to establish the relationship with Subjects
        public ICollection<Subject>? Subjects { get; set; }
        public ICollection<Student>? Students { get; set; }
    }
}
