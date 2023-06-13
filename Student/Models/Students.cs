using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student.Models
{
    public class Students
    {
        public Students()
        {
            Invoices = new HashSet<Invoices>();
        }


        [Key]
        public int Id { get; set; }

        [Display(Name =" أسم الطالب")]
        [MinLength(10, ErrorMessage = "the miniumm length is 10 characters"), MaxLength(80, ErrorMessage = "The maxiumm length is 80 characters")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = " عمر الطالب ")]
        [Range(7, 30, ErrorMessage = "Range between 7 and 25")]
        public int Age { get; set; }

        [Display(Name =" الجنس ")]
        public int gender { get; set; }

        [Display(Name = "رقم الهاتف")]
        public int phoneNo { get; set; }

        [Display(Name = " المستوى")]
        public int Level { get; set; }

        [Display(Name = " الايميل")]
        [EmailAddress(ErrorMessage = "The email is invalid ")]
        [Required(ErrorMessage = "The Email is empty")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = " كلمة السر ")]
        [Required(ErrorMessage = "The password is empty")]

        public string Password { get; set; } = string.Empty;
 
        [Display(Name ="اسم القسم")]
        public int? DepartmentIdFk { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
       
        [Display(Name = "اسم القسم")]
        [ForeignKey("DepartmentIdFk")]
        public virtual Departments? DepartmentIdFkNavigation { get; set; }
        public virtual ICollection<Invoices> Invoices { get; set; }

        public virtual List<Enrollement> ? StudentCourse { get; set; }

    }
}
