using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student.DataAccess;
using Student.Models;

namespace Student.Controllers
{
    public class LibraryController : Controller
    {
        private readonly stdDBContext _context;
        private IRepository<Libraries> _r;


        public LibraryController(stdDBContext context, IRepository<Libraries> r)
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

                TempData["LibraryActive"] = "active";

                var stdData = new StudentsViewModel();
                stdData.IDSort = OrderBy == "ID" ? "ID_desc" : "ID";
                stdData.NameSort = string.IsNullOrEmpty(OrderBy) ? "Name_desc" : "";
                temp = string.IsNullOrEmpty(temp) ? "" : temp.ToLower();
                var stud = (from std in _context.libraries.Include(a => a.DepartmentIdFkNavigation)
                            where (temp == "" || std.Name.ToLower().StartsWith(temp)) && (ID_search == 0 ? !std.Id.Equals(ID_search) : std.Id.Equals(ID_search))

                            select new Libraries
                            {
                                Id = std.Id,
                                Name = std.Name,
                                DepartmentIdFkNavigation = std.DepartmentIdFkNavigation,
                                DepartmentIdFk = std.DepartmentIdFk,
                                CreatedDate = std.CreatedDate,
                                UpdatedDate = std.UpdatedDate,
                                Type = std.Type,
                                Language = std.Language,
                                Status = std.Status,

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
                if(currentPage > totalPage) currentPage = totalPage;
                stdData.Library = stud;
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
        public IActionResult DeleteBook(int SID)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];


            if (TempData["LoginID"] != null)
            {
                _r.DeleteById(SID);
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
                _r.DeleteAll();
                TempData["MsgD"] = "تم حذف الجميع  بنجاح";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

          
        }

        public IActionResult AddBook()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                ViewBag.DepartmentIdFkNavigation = _context.departments.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

          
        } 
        public IActionResult EditBook(int SID)
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
        public IActionResult EditBook(int SID,Libraries book)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {

                ViewBag.DepartmentIdFkNavigation = _context.departments.ToList();

                Libraries books = _r.GetById(SID);
                books.Name = book.Name;
                books.Language = book.Language;
                books.Type = book.Type;
                books.DepartmentIdFk = book.DepartmentIdFk;
                books.Status = book.Status;
                books.UpdatedDate = DateTime.Now;
                if (books != null)
                {
                    _r.Editing(books);
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
        public IActionResult AddBook(Libraries book)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {

                ViewBag.DepartmentIdFkNavigation = _context.departments.ToList();
                Libraries books = new Libraries();
                books.Name = book.Name;
                books.Language = book.Language;
                books.DepartmentIdFk = book.DepartmentIdFk;
                books.Type = book.Type;
                books.Status = book.Status;
                books.CreatedDate = DateTime.Now;
                books.UpdatedDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _r.Create(books);
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
