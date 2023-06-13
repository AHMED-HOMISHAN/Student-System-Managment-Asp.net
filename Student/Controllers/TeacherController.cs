using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student.DataAccess;
using Student.Models;

namespace Student.Controllers
{
    public class TeacherController : Controller
    {
        private readonly stdDBContext _context;
        private IRepository<Teachers> _r;

        public TeacherController(stdDBContext context, IRepository<Teachers> r)
        {
            _context = context;
            _r = r;
        }
        public IActionResult Index()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                return RedirectToAction("TeacherList", "Teacher");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


          
        }

        public IActionResult TeacherList(string temp = "", int ID_search = 0, int phone_search = 0, string OrderBy ="", int currentPage = 1)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                TempData["TeacherLActive"] = "active";

                var teacherData = new StudentsViewModel();

                temp = string.IsNullOrEmpty(temp) ? "" : temp.ToLower();
                teacherData.IDSort = OrderBy == "ID" ? "ID_desc" : "ID";
                teacherData.NameSort = string.IsNullOrEmpty(OrderBy) ? "Name_desc" : "";

                var teach = (from tch in _context.teachers.Include(a => a.DepartmentIdFkNavigation)
                             where (temp == "" || tch.Name.ToLower().StartsWith(temp)) && (phone_search == 0 ? !tch.phoneNo.Equals(phone_search) : tch.phoneNo.Equals(phone_search)) && (ID_search == 0 ? !tch.Id.Equals(ID_search) : tch.Id.Equals(ID_search))
                             select new Teachers
                             {
                                 Id = tch.Id,
                                 Name = tch.Name,
                                 Age = tch.Age,
                                 Balance = tch.Balance,
                                 gender = tch.gender,
                                 DepartmentIdFkNavigation = tch.DepartmentIdFkNavigation,
                                 Email = tch.Email,
                                 DepartmentIdFk = tch.DepartmentIdFk,
                                 Password = tch.Password,
                                 phoneNo = tch.phoneNo,
                                 CreatedDate = tch.CreatedDate,
                                 UpdatedDate = tch.UpdatedDate,
                             }
                  );
                switch (OrderBy)
                {
                    case "ID": teach = teach.OrderBy(a => a.Id); break;
                    case "ID_desc": teach = teach.OrderByDescending(a => a.Id); break;
                    case "Name_desc": teach = teach.OrderByDescending(a => a.Name); break;
                    default: teach.OrderBy(a => a.Name); break;
                }

                int totalRecords = teach.Count();
                int pageSize = 5;
                int totalPage = (int)Math.Ceiling(totalRecords / (double)pageSize);
                teach = teach.Skip((currentPage - 1) * pageSize).Take(pageSize);

                if (currentPage <= 1) currentPage = 1;
                if (currentPage > totalPage) currentPage = totalPage;

                teacherData.Teacher = teach;
                teacherData.CurrentPage = currentPage;
                teacherData.PageSize = pageSize;
                teacherData.TotalPages = totalPage;
                teacherData.Term = temp;
                teacherData.orderBy = OrderBy;

                return View(teacherData);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult TeacherView(int SID,string Dep_TEACH)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                if (_r.GetById(SID) != null)
                {

                    TempData["Dep_TEACH"] = Dep_TEACH;
                    return View(_r.GetById(SID));
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

       
       
        public IActionResult AddTeacher()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                TempData["TeacherAActive"] = "active";
                ViewBag.DepartmentIdFkNavigation = _context.departments.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        } 
        [HttpPost]
        public IActionResult AddTeacher(Teachers fteacher)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {

                ViewBag.DepartmentIdFkNavigation = _context.departments.ToList();
                Teachers teacher = new Teachers();
                teacher.Name = fteacher.Name;
                teacher.Email = fteacher.Email;
                teacher.Password = fteacher.Password;
                teacher.Age = fteacher.Age;
                teacher.Balance = fteacher.Balance;
                teacher.JoinedDateTime = fteacher.JoinedDateTime;
                teacher.Experience = fteacher.Experience;
                teacher.gender = fteacher.gender;
                teacher.phoneNo = fteacher.phoneNo;
                teacher.DepartmentIdFk = fteacher.DepartmentIdFk;
                teacher.CreatedDate = DateTime.Now;
                teacher.UpdatedDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _r.Create(teacher);
                    TempData["MsgS"] = "تم إدخال البيانات بنجاح";
                    return RedirectToAction(nameof(TeacherList));
                }

                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        } 
        public IActionResult EditTeacher(int SID)
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
        public IActionResult EditTeacher(int SID,Teachers fteacher)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                ViewBag.DepartmentIdFkNavigation = _context.departments.ToList();

                Teachers teacher = _r.GetById(SID);
                teacher.Name = fteacher.Name;
                teacher.Email = fteacher.Email;
                teacher.Password = fteacher.Password;
                teacher.Age = fteacher.Age;
                teacher.Balance = fteacher.Balance;
                teacher.gender = fteacher.gender;
                teacher.phoneNo = fteacher.phoneNo;
                teacher.DepartmentIdFk = fteacher.DepartmentIdFk;

                teacher.UpdatedDate = DateTime.Now;
                if (teacher != null)
                {
                    _r.Editing(teacher);
                    TempData["MsgS"] = "تم تحديث البيانات بنجاح";

                    return RedirectToAction(nameof(TeacherList));
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

       
        } 
        public IActionResult TeacherGrid()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                ViewBag.DepartmentIdFkNavigation = _context.departments.ToList();

                return View(_r.GetAll());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


           
        }
    }
}
