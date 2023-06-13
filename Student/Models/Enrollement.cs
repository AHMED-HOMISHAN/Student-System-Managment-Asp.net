using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student.Models
{
    public class Enrollement
    {
        [Required(ErrorMessage ="You must choose a student")]
        [Display(Name="Student Name")]
        public int studentIdFk { get; set; }

        [ForeignKey("studentIdFk")]
        public virtual Students StudentFKNavigation { get; set; }
        [Required(ErrorMessage = "You must choose a course")]
        [Display(Name = "Course Name")]
        public int courseIdFk { get; set; }

        [ForeignKey("courseIdFk")]
        public virtual Courses CourseFKNavigation { get; set; }

        [Column("Marks")]
        [Range(0, 100, ErrorMessage = "Range between 0 and 100")]
        public double courseScore { get; set; }
    }
}
