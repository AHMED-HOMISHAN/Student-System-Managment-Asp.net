namespace Student.Models
{
    public class Courses
    {
        public int Id { get; set; }
        public string Name { get; set; }= String.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public List<Enrollement> ? StudentsCourses { get; set; }
    }
}
