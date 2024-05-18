using System.ComponentModel.DataAnnotations.Schema;

namespace SiliconApp.Models
{
    [Table("CourseInfo")]  // Specify the table name for the CourseInfo entity
    public class CourseInfo
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public int NumberOfHours { get; set; }
        public int Rate { get; set; }
        public int NumberOfLikes { get; set; }
    }
}
