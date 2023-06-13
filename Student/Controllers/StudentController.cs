using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student.DataAccess;
using Student.Models;
namespace Student.Controllers
{
    public class StudentController : Controller
    {
        private readonly stdDBContext _context;
        private  IRepository<Students> _r;

        public StudentController(stdDBContext context ,IRepository<Students> r)
        {
            _r = r;
            _context = context;
        }
        public IActionResult Index()
        {

            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] !=null)
            {
                TempData["AdminActive"] = "active";
                TempData["Total_Invoices"] = _context.invoices.Count().ToString();


                TempData["std_count"] = _context.students.Count();
                TempData["teach_count"] = _context.teachers.Count();
                TempData["dep_count"] = _context.departments.Count();

                return View(_context.students.Where(a=>a.Id <= 5));
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        } 
        public IActionResult Profile(int SID)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];
            if (TempData["LoginID"] != null)
            {
                return View(_context.students.Include(c => c.StudentCourse).ThenInclude(c => c.CourseFKNavigation).Where(s => s.Id == SID).FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        } 
       
        [HttpGet]
        public IActionResult StudentDetails(int SID,string Dep_ST)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {

                if (_r.GetById(SID) != null)
                {
                    TempData["Dep_ST"] = Dep_ST;
                    return View(_context.students.Include(c=>c.StudentCourse).ThenInclude(c=>c.CourseFKNavigation).Where(s=>s.Id==SID).FirstOrDefault());
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }




        }
        public IActionResult StudentsList(string temp="",int ID_search=0,int phone_search=0,string OrderBy="",int currentPage=1)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];



            if (TempData["LoginID"] != null)
            {


                TempData["StudentLActive"] = "active";
                var stdData = new StudentsViewModel();
                stdData.IDSort = OrderBy == "ID" ? "ID_desc" : "ID";
                stdData.NameSort = string.IsNullOrEmpty(OrderBy) ? "Name_desc" : "";
                temp = string.IsNullOrEmpty(temp) ? "" : temp.ToLower();
                var stud = (from std in _context.students.Include(a => a.DepartmentIdFkNavigation)
                            where (temp == "" || std.Name.ToLower().StartsWith(temp)) && (phone_search == 0 ? !std.phoneNo.Equals(phone_search) : std.phoneNo.Equals(phone_search)) && (ID_search == 0 ? !std.Id.Equals(ID_search) : std.Id.Equals(ID_search))
                            select new Students
                            {
                                Id = std.Id,
                                Name = std.Name,
                                Age = std.Age,
                                gender = std.gender,
                                DepartmentIdFkNavigation = std.DepartmentIdFkNavigation,
                                Email = std.Email,
                                DepartmentIdFk = std.DepartmentIdFk,
                                Level = std.Level,
                                Password = std.Password,
                                phoneNo = std.phoneNo,
                                CreatedDate = std.CreatedDate,
                                UpdatedDate = std.UpdatedDate,
                                Invoices = std.Invoices,
                            }
                    );
                switch (OrderBy)
                {
                    case "ID": stud = stud.OrderBy(a => a.Id); break;
                    case "ID_desc": stud = stud.OrderByDescending(a => a.Id); break;
                    case "Name_desc": stud = stud.OrderByDescending(a => a.Name); break;
                    default: stud.OrderBy(a => a.Name); break;
                }

                int totalRecords = stud.Count();
                int pageSize = 5;
                int totalPage = (int)Math.Ceiling(totalRecords / (double)pageSize);
                stud = stud.Skip((currentPage - 1) * pageSize).Take(pageSize);

                stdData.Student = stud;
                stdData.CurrentPage = currentPage;
                stdData.PageSize = pageSize;
                stdData.TotalPages = totalPage;
                stdData.Term = temp;
                stdData.orderBy = OrderBy;
                return View(stdData);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
      
        public IActionResult StudentGrid()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {
                TempData["StudentGActive"] = "active";

                ViewBag.DepartmentIdFkNavigation = _context.departments.ToList();

                return View(_r.GetAll());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

           
        }

        public IActionResult AddStudent()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {
                TempData["StudentAActive"] = "active";

                ViewBag.DepartmentIdFkNavigation = _context.departments.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

           
        }

        [HttpPost]
        public IActionResult AddStudent(Students ? std)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {


                ViewBag.DepartmentIdFkNavigation = _context.departments.ToList();

                Students student = new Students();
                student.Name = std.Name;
                student.Email = std.Email;
                student.Password = std.Password;
                student.Age = std.Age;
                student.Level = std.Level;
                student.DepartmentIdFk = std.DepartmentIdFk;
                student.gender = std.gender;
                student.phoneNo = std.phoneNo;
                student.CreatedDate = DateTime.Now;
                student.UpdatedDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _r.Create(student);
                    TempData["MsgS"] = "تم إدخال البيانات بنجاح";
                    return RedirectToAction(nameof(StudentsList));
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
        } 
        public IActionResult EditStudent(int SID)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {

                if (_r.GetById(SID) != null)
                {
                    ViewBag.DepartmentIdFkNavigation = _context.departments.ToList();
                    return View(_r.GetById(SID));

                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }  

        [HttpPost]
        public IActionResult EditStudent(int SID, Students std)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];



            if (TempData["LoginID"] != null)
            {
                ViewBag.DepartmentIdFkNavigation = _context.departments.ToList();


                Students student = _r.GetById(SID);
                student.Name = std.Name;
                student.Email = std.Email;
                student.Password = std.Password;
                student.DepartmentIdFk = std.DepartmentIdFk;
                student.Level = std.Level;
                student.Age = std.Age;
                student.gender = std.gender;
                student.phoneNo = std.phoneNo;
                student.UpdatedDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _r.Editing(student);
                    TempData["MsgS"] = "تم تحديث البيانات بنجاح";
                    return RedirectToAction(nameof(StudentsList));

                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


            
        }
    } 
  
}


