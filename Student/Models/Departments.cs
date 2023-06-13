using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student.Models
{
    public class Departments
    {
        public Departments()
        {
            Libraries = new HashSet<Libraries>();
            Students = new HashSet<Students>();
            Teachers = new HashSet<Teachers>();
        }
        [Key]
        public int Id { get; set; }
        [MinLength(2, ErrorMessage = "the miniumm length is 2 characters"), MaxLength(80, ErrorMessage = "The maxiumm length is 80 characters")]
        [Display(Name = "أسم القسم ")]
        public string Name { get; set; } = String.Empty;
        [MinLength(10, ErrorMessage = "the miniumm length is 5 characters"), MaxLength(200, ErrorMessage = "The maxiumm length is 200 characters")]
        [Display(Description = " معلومات القسم ")]
        public string Description { get; set; } = String.Empty;
        

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
       
        public virtual ICollection<Teachers> Teachers { get; set; }
        public virtual ICollection<Libraries> Libraries { get; set; }
        public virtual ICollection<Students> Students { get; set; }

    }
}
