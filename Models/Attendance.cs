using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EyeAttend.Models
{
    [Table("Attendance")] // Explicitly sets the table name in the database
    public class Attendance
    {
        [Key] // Marks the property as the primary key
        public int Id { get; set; }

        [ForeignKey("SubjectId")] // Indicates a foreign key relationship to the Subject table
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } // Navigation property to the associated Subject

        [Column(TypeName = "date")] // Stores only the date part (without time)
        public DateTime AttendanceDate { get; set; }

        [ForeignKey("SessionYearId")] // Indicates a foreign key relationship to the SessionYear table
        public int SessionYearId { get; set; }
        public SessionYear SessionYear { get; set; } // Navigation property to the associated SessionYear

        // Timestamps (Optional but recommended for tracking record creation and updates)
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
