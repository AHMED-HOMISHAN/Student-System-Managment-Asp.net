using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student.Models
{
    public class Teachers
    {
        public Teachers()
        {
            Subjects = new HashSet<Subjects>();
        }

        [Key]
        public int Id { get; set; }
        
        [MinLength(10, ErrorMessage = "the miniumm length is 10 characters"), MaxLength(80, ErrorMessage = "The maxiumm length is 80 characters")]
        [Display(Name ="أسم المدرس")]
        public string Name { get; set; } = string.Empty;
        
        [Range(7, 22, ErrorMessage = "Range between 7 and 22")]
        [Display(Name = "عمر المدرس")]
        public int Age { get; set; }
        
        [Display(Name = " الجنس")]
        public int gender { get; set; }
        
        [Display(Name = "رقم الهاتف")]
        public int phoneNo { get; set; }

        [Display(Name = "راتب")]
        public double Balance { get; set; }
        
        [Display(Name = "الايميل")]
        [EmailAddress(ErrorMessage = "The email is invalid "),]
        public string Email { get; set; } = string.Empty;
        
        [Display(Name = "كلمة السر")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "تاريخ بداية العقد")]
        [Required]
        public DateTime JoinedDateTime { get; set; } = DateTime.Now;

        [Display(Name = " الخبرات")]
        [Required]
        public String Experience { get; set; } =String.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        [Display(Name = " القسم")]

        public int? DepartmentIdFk { get; set; }
        [ForeignKey("DepartmentIdFk")]
        [Display(Name = " القسم")]

        public virtual Departments? DepartmentIdFkNavigation { get; set; }
        public virtual ICollection<Subjects> Subjects { get; set; }
    }
}
