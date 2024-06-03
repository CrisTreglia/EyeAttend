using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EyeAttend.Models
{
    [Table("StudentPhotos")]
    public class StudentPhoto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageURL { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
