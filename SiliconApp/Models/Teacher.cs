using System.ComponentModel.DataAnnotations.Schema;

namespace SiliconApp.Models
{
  // Specify the table name for the Course entity
    public class Teacher
    {
        public int Id { get; set; }

        public string TeacherName { get; set; } = null!;

        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public ICollection<TeacherCourse>? TeacherCourses { get; set; }
    }
}
