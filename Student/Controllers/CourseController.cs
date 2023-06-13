using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student.DataAccess;
using Student.Models;

namespace Student.Controllers
{
    public class CourseController : Controller
    {
        private readonly stdDBContext _context;
        private IRepository<Courses> _r;


        public CourseController(stdDBContext context, IRepository<Courses> r)
        {
            _context = context;
            _r = r;
        }
        public IActionResult Index(string temp = "", int ID_search = 0, string OrderBy = "", int currentPage = 1)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {

                TempData["CourseActive"] = "active";

                var stdData = new StudentsViewModel();
                stdData.IDSort = OrderBy == "ID" ? "ID_desc" : "ID";
                stdData.NameSort = string.IsNullOrEmpty(OrderBy) ? "Name_desc" : "";
                temp = string.IsNullOrEmpty(temp) ? "" : temp.ToLower();
                var stud = (from std in _context.courses.Include(c=>c.StudentsCourses)
                            where (temp == "" || std.Name.ToLower().StartsWith(temp)) && (ID_search == 0 ? !std.Id.Equals(ID_search) : std.Id.Equals(ID_search))

                            select new Courses
                            {
                                Id = std.Id,
                                Name = std.Name,
                                StudentsCourses=std.StudentsCourses,
                                CreatedDate = std.CreatedDate,
                                UpdatedDate = std.UpdatedDate,
                              
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

                if (currentPage <= 1) currentPage = 1;
                if (currentPage > totalPage) currentPage = totalPage;

                stdData.Course = stud;
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
       

        public IActionResult AddCourse()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        } 
        public IActionResult AddMarks()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                ViewBag.StudentFkNavigation = _context.students;
                ViewBag.courseFkNavigation = _context.courses;
             
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        public IActionResult EditMarks(int SID,int courseID)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                ViewBag.StudentFkNavigation = _context.students;
                ViewBag.courseFkNavigation = _context.courses.Where(a=>a.Id!=4);
                return View(_context.Enrollement.Where(e=>e.studentIdFk == SID && e.courseIdFk == courseID).FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        [HttpPost]
        public IActionResult AddMarks(Enrollement enroll)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                Enrollement en = new Enrollement();
                en.studentIdFk = enroll.studentIdFk;
                en.courseScore=enroll.courseScore;
                en.courseIdFk = enroll.courseIdFk;
                if(en.courseScore <=150)
                {
                    var checkCourse = _context.Enrollement.Where(e=>e.studentIdFk == en.studentIdFk && e.courseIdFk == en.courseIdFk).FirstOrDefault();
                    if (checkCourse !=null)
                    {
                        TempData["MsgD"] = " هذا الطالب تم الاشتراك في هذا الكورس مسبقا  ";
                        return RedirectToAction("AddMarks");
                    }
                    else 
                    {
                        _context.Enrollement.Add(en);
                        _context.SaveChanges();
                        TempData["MsgS"] = "تم إدخال البيانات بنجاح";

                        return RedirectToAction("AddMarks");
                    }

                }
                else
                {
                    TempData["MsgD"] = "  الدرجة المدخله أكبر من 150 ";
                    return RedirectToAction("AddMarks");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        } 
        [HttpPost]
        public IActionResult EditMarks(int SID,int courseID, Enrollement enroll)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                Enrollement en = _context.Enrollement.Where(e => e.studentIdFk == SID && e.courseIdFk == courseID).FirstOrDefault();
                en.studentIdFk = enroll.studentIdFk;
                en.courseScore=enroll.courseScore;
                en.courseIdFk = enroll.courseIdFk;
                if(en != null)
                {
                    _context.Enrollement.Update(en);
                    _context.SaveChanges();

                    TempData["MsgS"] = "تم تحديث البيانات بنجاح";

                    return RedirectToAction("AddMarks");
                }
                return RedirectToAction("AddMarks");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
        public IActionResult EditCourse(int SID)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {

                if (_r.GetById(SID) != null)
                {

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
        public IActionResult EditCourse(int SID, Courses Course)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                

                Courses Courses = _r.GetById(SID);
                Courses.Name = Course.Name;
         
                Courses.UpdatedDate = DateTime.Now;
                if (Courses != null)
                {
                    _r.Editing(Courses);
                    TempData["MsgS"] = "تم تحديث البيانات بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
        [HttpPost]
        public IActionResult AddCourse(Courses Course)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {

                Courses Courses = new Courses();
                Courses.Name = Course.Name;
               
                Courses.CreatedDate = DateTime.Now;
                Courses.UpdatedDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _r.Create(Courses);
                    TempData["MsgS"] = "تم إدخال البيانات بنجاح";
                    return RedirectToAction(nameof(Index));
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
