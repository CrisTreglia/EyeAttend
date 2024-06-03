using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EyeAttend.Models
{
    [Table("SessionYears")] // Explicitly sets the table name in the database
    public class SessionYear
    {
        [Key] // Marks the property as the primary key
        public int Id { get; set; }

        [Column(TypeName = "date")] // Stores only the date part (without time)
        public DateTime SessionStartYear { get; set; }

        [Column(TypeName = "date")] // Stores only the date part (without time)
        public DateTime SessionEndYear { get; set; }

        // Navigation properties to establish relationships (if needed)
        public ICollection<Student>? Students { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
    }
}
