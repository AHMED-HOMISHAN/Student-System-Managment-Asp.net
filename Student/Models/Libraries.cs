using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student.Models
{
    public class Libraries
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "أسم المكتبة")]
        [Required]
        public String Name { get; set; }

        [Display(Name = " اللغة")]
        [Required]
        public String Language { get; set; }


        [Display(Name = "النوع ")]
        [Required]
        public String Type { get; set; }

        [Display(Name = " الحالة ")]
        [Required]
        public String Status { get; set; }
        [Display(Name = " القسم ")]

        public int? DepartmentIdFk { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        public DateTime UpdatedDate { get; set; }= DateTime.Now;
        [ForeignKey("DepartmentIdFk")]
        public virtual Departments? DepartmentIdFkNavigation { get; set; }
    }
}
