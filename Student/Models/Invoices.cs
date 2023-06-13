using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student.Models
{
    public class Invoices
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = " الفئة ")]

        public String Category { get; set; } = String.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "من العنوان")]

        public string FromAddress { get; set; } = String.Empty;
        [Required]
        [Display(Name = "العنوان المرسل اليه")]

        [Column("InoviceTo")]
        public string ToAddress { get; set; } = String.Empty;
        [Required]
        [Display(Name = "المبلغ")]

        public double InoviceAmount { get; set; }

        [Range(typeof(DateTime),"1-1-2023","1-1-2030")]
        public DateTime DueDate { get; set; } =DateTime.Now;
        [Required]
        [Display(Name = "الحالة")]

        public String Status { get; set; } = String.Empty;
        [Display(Name = "وصف")]

        public string Description { get; set; } =String.Empty;
        [Required]
        [Display(Name = "السعر")]

        public double Rate { get; set; }
        [Display(Name = "الخصم")]

        public double Discount { get; set; }
        [Required]
        [Display(Name = "الكمية")]
        public double Quantity { get; set; }

        [Display(Name = "أسم الطالب")]

        public int? StudentFk { get; set; }
        [Display(Name = "أسم الطالب")]

        [ForeignKey("StudentFk")]
        public virtual Students? StudentFkNavigation { get; set; }


    }
}
