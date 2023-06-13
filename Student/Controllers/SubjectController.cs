using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student.DataAccess;
using Student.Models;

namespace Student.Controllers
{
    public class SubjectController : Controller
    {
        private readonly stdDBContext _context;
        private IRepository<Subjects> _r;

        public SubjectController(stdDBContext context, IRepository<Subjects> r)
        {
            _context = context;
            _r = r;
        }
        public IActionResult Index(string temp = "", int ID_search = 0, string OrderBy = "", int currentPage = 1)
        {
            TempData["SubjectLActive"] = "active";
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];
            if (TempData["LoginID"] != null)
            {
                var subData = new StudentsViewModel();
                subData.IDSort = OrderBy == "ID" ? "ID_desc" : "ID";
                subData.NameSort = string.IsNullOrEmpty(OrderBy) ? "Name_desc" : "";
                temp = string.IsNullOrEmpty(temp) ? "" : temp.ToLower();
                var stud = (from std in _context.subjects.Include(a => a.TeacherIdFkNavigation)
                            where (temp == "" || std.Name.ToLower().StartsWith(temp)) && (ID_search == 0 ? !std.Id.Equals(ID_search) : std.Id.Equals(ID_search))

                            select new Subjects
                            {
                                Id = std.Id,
                                Name = std.Name,
                                Description = std.Description,
                                CreatedDate = std.CreatedDate,
                                UpdatedDate = std.UpdatedDate,
                                TeacherIdFk = std.TeacherIdFk,
                                TeacherIdFkNavigation = std.TeacherIdFkNavigation,
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

                subData.Subject = stud;
                subData.CurrentPage = currentPage;
                subData.PageSize = pageSize;
                subData.TotalPages = totalPage;
                subData.Term = temp;
                subData.orderBy = OrderBy;

                return View(subData);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult SubjectDelete(int SID)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];


            if (TempData["LoginID"] != null)
            {

                _r.DeleteById(SID);
                _context.SaveChanges();
                TempData["MsgD"] = "تم حذف البيانات بنجاح";


                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        public IActionResult DeleteAll()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];


            if (TempData["LoginID"] != null)
            {

                _r.DeleteAll("subjects");
                TempData["MsgD"] = "تم حذف الجميع  بنجاح";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult AddSubject()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {

                TempData["SubjectAActive"] = "active";

                ViewBag.TeacherIdFkNavigation = _context.teachers.ToList();

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        } 
        [HttpPost]
        public IActionResult AddSubject(Subjects sub)
        {

            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {

                ViewBag.TeacherIdFkNavigation = _context.teachers.ToList();
                Subjects subject = new Subjects();
                subject.Name = sub.Name;
                subject.TeacherIdFk = sub.TeacherIdFk;
                subject.Description = sub.Description;
                subject.CreatedDate = DateTime.Now;
                subject.UpdatedDate = DateTime.Now;

                if (ModelState.IsValid)
                {
                    _r.Create(subject);
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
        public IActionResult EditSubject(int SID)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {

                if (_r.GetById(SID) != null)
                {
                    ViewBag.TeacherIdFkNavigation = _context.teachers.ToList();
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
        public IActionResult EditSubject(int SID,Subjects sub)
        {

            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];



            if (TempData["LoginID"] != null)
            {
                ViewBag.TeacherIdFkNavigation = _context.teachers.ToList();

                Subjects subject = _r.GetById(SID);
                subject.Name = sub.Name;
                subject.TeacherIdFk = sub.TeacherIdFk;
                subject.Description = sub.Description;
                subject.UpdatedDate = DateTime.Now;

                if (subject != null)
                {
                    _r.Editing(subject);
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
    }
}
