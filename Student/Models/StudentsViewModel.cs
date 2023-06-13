namespace Student.Models
{
    public class StudentsViewModel
    {
        public IQueryable<Students> Student { get; set; }
        public IQueryable<Teachers> Teacher { get; set; }
        public IQueryable<Libraries> Library { get; set; }
        public IQueryable<Invoices> Invoice { get; set; }
        public IQueryable<Subjects> Subject { get; set; }
        public IQueryable<Departments> Department { get; set; }
        public IQueryable<Courses> Course { get; set; }
        public string orderBy { get; set; }
        public string IDSort { get; set; }
        public string NameSort { get; set; }
        public string Term { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
