using Microsoft.AspNetCore.Mvc;
using Student.DataAccess;
using Student.Models;

namespace depart.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly stdDBContext _context;
        private IRepository<Departments> _r;


        public DepartmentController(stdDBContext context, IRepository<Departments> r)
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

                TempData["DepartmentLActive"] = "active";
                var stdData = new StudentsViewModel();
                stdData.IDSort = OrderBy == "ID" ? "ID_desc" : "ID";
                stdData.NameSort = string.IsNullOrEmpty(OrderBy) ? "Name_desc" : "";
                temp = string.IsNullOrEmpty(temp) ? "" : temp.ToLower();
                var stud = (from std in _context.departments
                            where (temp == "" || std.Name.ToLower().StartsWith(temp)) && (ID_search == 0 ? !std.Id.Equals(ID_search) : std.Id.Equals(ID_search))

                            select new Departments
                            {
                                Id = std.Id,
                                Name = std.Name,
                                Description = std.Description,
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
                if(currentPage > totalPage) currentPage = totalPage;

                stdData.Department = stud;
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

        public IActionResult EditDepartment(int SID)
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
        public IActionResult EditDepartment(int SID, Departments dep)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];



            if (TempData["LoginID"] != null)
            {

                Departments depart = _r.GetById(SID);
                depart.Name = dep.Name;
                depart.Description = dep.Description;
                depart.UpdatedDate = DateTime.Now;

                if (depart != null)
                {
                    _r.Editing(depart);
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
        public IActionResult AddDepartment()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {
                TempData["DepartmentAActive"] = "active";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


           
        }
        [HttpPost]
        public IActionResult AddDepartment(Departments dep)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {
                Departments depart = new Departments();
                depart.Name = dep.Name;
                depart.Description = dep.Description;
                depart.CreatedDate = DateTime.Now;
                depart.UpdatedDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _r.Create(depart);
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
