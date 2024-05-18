using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SiliconApp.Models
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }



        public string CourseName { get; set; } = null!;

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal PriceAfterDiscount { get; set; }
        public int NumberOfHours { get; set; }
        public int Rate { get; set; }
        public int NumberOfLikes { get; set; }

        [Display(Name = "downloadable resources")]
        public int DowinloadResource { get; set; }

        [NotMapped]
        public decimal NumberOfLikesResult { get; set; }


        public string? ImagePath { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string CoursDec { get; set; }
 
        public int Aritc { get; set; }
        public int iDowinload { get; set; }

        [Display(Name = "Full Time")]
        public bool Time { get; set; } = true;
        public bool Certifacate { get; set; }

        public ICollection<UserSavedItem>? UserSavedItems { get; set; }


        //boolean Time?true ==Full : flase half time
        //boolean Certifcate ?true ==cer : flase  without certificate



        public virtual List<CourseDetails> CourseDetails { get; set; } = new();
        public virtual List<CourseLearn> CourseLearns { get; set; } = new();
        public virtual List<TeacherCourse> TeacherCourses { get; set; } = new();


        [NotMapped]
        public List<SelectList>? selectListItems { get; set; }
        [NotMapped]
        public List<Teacher>? Teachers { get; set; } = new();


 

        [NotMapped]
        public bool IsSaved { get; set; }

        [NotMapped]
        public bool IsLiked { get; set; }

        [NotMapped]
        public bool IsBestSeller { get; set; }
    }
}
