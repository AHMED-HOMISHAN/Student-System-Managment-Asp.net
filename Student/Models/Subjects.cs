using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student.Models
{
    public class Subjects
    {
      
        [Key]
        public int Id { get; set; }
        [Display(Name ="أسم المادة")]
        [MinLength(5, ErrorMessage = "the miniumm length is 5 characters"), MaxLength(80, ErrorMessage = "The maxiumm length is 80 characters")]
        public string Name { get; set; }=String.Empty;
        [Display(Name = "معلومات المادة")]
        [MinLength(10, ErrorMessage = "the miniumm length is 10 characters"), MaxLength(200, ErrorMessage = "The maxiumm length is 200 characters")]
        public string Description { get; set; } = String.Empty;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [Display(Name="الاستاذ")]
        public int? TeacherIdFk { get; set; }
        [ForeignKey("TeacherIdFk")]
        public virtual Teachers? TeacherIdFkNavigation { get; set; }
    }
}
