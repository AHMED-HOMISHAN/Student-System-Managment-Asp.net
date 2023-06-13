using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student.DataAccess;
using Student.Models;

namespace Student.Controllers
{
    public class InvoiceController : Controller
    {

        private readonly stdDBContext _context;
        private IRepository<Invoices> _r;


        public InvoiceController(stdDBContext context, IRepository<Invoices> r)
        {
            _context = context;
            _r = r;
        }
        public IActionResult Index(int ID_search=0 , string OrderBy = "", int currentPage = 1,int username=0)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {
                TempData["Total_Invoices"] = _context.invoices.Count().ToString();
                TempData["Total_Invoices_rate"] = _context.invoices.Sum(a => a.Rate).ToString();
                TempData["Paid_Invoices"] = _context.invoices.Where(a => a.Status == "Paid").Count().ToString();
                TempData["Paid_Invoices_rate"] =   _context.invoices.Where(a => a.Status == "Paid").Sum(a => a.Rate).ToString();
                TempData["Over_Invoices"] = _context.invoices.Where(a => a.Status == "Overdue").Count().ToString();
                TempData["Over_Invoices_rate"] = _context.invoices.Where(a => a.Status == "Overdue").Sum(a=>a.Rate).ToString(); 
                TempData["Cancelled_Invoices"] = _context.invoices.Where(a => a.Status == "Cancelled").Count().ToString();
                TempData["Cancelled_Invoices_rate"] = _context.invoices.Where(a => a.Status == "Cancelled").Sum(a=>a.Rate).ToString();


                TempData["InvoicesLActive"] = "active";
                var stdData = new StudentsViewModel();
               
                var stud = (from std in _context.invoices.Include(a => a.StudentFkNavigation)
                            where ( ID_search == 0 ? !std.Id.Equals(ID_search) : std.Id.Equals(ID_search) ) && (username == 0 ? !std.StudentFkNavigation.Id.Equals(username) : std.StudentFkNavigation.Id.Equals(username) )
                            select new Invoices
                            {
                                Id = std.Id,
                                StudentFkNavigation = std.StudentFkNavigation,
                                Category = std.Category,
                                InoviceAmount = std.InoviceAmount,
                                StudentFk = std.StudentFk,
                                Status = std.Status,
                                DueDate = std.DueDate,
                                CreatedDate = std.CreatedDate,
                                Rate = std.Rate,
                            }
                    );

               switch (OrderBy)
                {
                    case "ID_desc": stud = stud.OrderByDescending(a => a.Id); break;
                    default: stud = stud.OrderBy(a => a.Id); break;
                }

                int totalRecords = stud.Count();
                int pageSize = 5;
                int totalPage = (int)Math.Ceiling(totalRecords / (double)pageSize);
                stud = stud.Skip((currentPage - 1) * pageSize).Take(pageSize);

                if (currentPage <= 1) currentPage = 1;
                if (currentPage > totalPage) currentPage = totalPage;

                stdData.Invoice = stud;
                stdData.CurrentPage = currentPage;
                stdData.PageSize = pageSize;
                stdData.TotalPages = totalPage;
                stdData.orderBy = OrderBy;

                return View(stdData);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


         
        } 
        public IActionResult InvoicesPaid()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {

                TempData["Total_Invoices"] = _context.invoices.Count().ToString();
                TempData["Total_Invoices_rate"] = _context.invoices.Sum(a => a.Rate).ToString();
                TempData["Paid_Invoices"] = _context.invoices.Where(a => a.Status == "Paid").Count().ToString();
                TempData["Paid_Invoices_rate"] = _context.invoices.Where(a => a.Status == "Paid").Sum(a => a.Rate).ToString();
                TempData["Over_Invoices"] = _context.invoices.Where(a => a.Status == "Overdue").Count().ToString();
                TempData["Over_Invoices_rate"] = _context.invoices.Where(a => a.Status == "Overdue").Sum(a => a.Rate).ToString();
                TempData["Cancelled_Invoices"] = _context.invoices.Where(a => a.Status == "Cancelled").Count().ToString();
                TempData["Cancelled_Invoices_rate"] = _context.invoices.Where(a => a.Status == "Cancelled").Sum(a => a.Rate).ToString();



                TempData["InvoicesLActive"] = "active";
                return View(_context.invoices.Where(s => s.Status == "Paid").Include(a => a.StudentFkNavigation).ToList());

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
} 
        public IActionResult InvoiceOverdue()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {


                TempData["InvoicesLActive"] = "active";
                return View(_context.invoices.Where(s => s.Status == "Overdue").Include(a => a.StudentFkNavigation).ToList());

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        } 
        public IActionResult InvoiceDraft()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {
                TempData["InvoicesLActive"] = "active";
                return View(_context.invoices.Where(s => s.Status == "Draft").Include(a => a.StudentFkNavigation).ToList());

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        } 
      
        public IActionResult InvoiceCancelled()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            TempData["Cancelled_Invoices"] = _context.invoices.Where(a => a.Status == "Cancelled").Count();


            if (TempData["LoginID"] != null)
            {
                TempData["InvoicesLActive"] = "active";
                return View(_context.invoices.Where(s => s.Status == "Cancelled").Include(a => a.StudentFkNavigation).ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

           
        } 

 

        public IActionResult AddInvoice()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];



            if (TempData["LoginID"] != null)
            {

                TempData["InvoicesAActive"] = "active";

                ViewBag.StudentFkNavigation = _context.students.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
        [HttpPost]
        public IActionResult AddInvoice(Invoices inv)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];



            if (TempData["LoginID"] != null)
            {

                ViewBag.StudentFkNavigation = _context.students.ToList();
                Invoices invoice = new Invoices();
                invoice.StudentFk = inv.StudentFk;
                invoice.Status = inv.Status;
                invoice.Description = inv.Description;
                invoice.Rate = inv.Rate;
                invoice.FromAddress = inv.FromAddress;
                invoice.ToAddress = inv.ToAddress;
                invoice.Discount = inv.Discount;
                invoice.DueDate = inv.DueDate;
                invoice.CreatedDate = DateTime.Now;
                invoice.Category = inv.Category;
                invoice.InoviceAmount = inv.InoviceAmount;
                invoice.Quantity = inv.Quantity;
                if (ModelState.IsValid)
                {
                    _r.Create(invoice);
                    TempData["MsgS"] = "تم اضافات البيانات بنجاح";
                    return RedirectToAction("Index");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }  
        public IActionResult EditInvoice(int SID)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];



            if (TempData["LoginID"] != null)
            {

                if (_r.GetById(SID) != null)
                {
                    ViewBag.StudentFkNavigation = _context.students.ToList();

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
        public IActionResult EditInvoice(int SID,Invoices inv)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {


                ViewBag.StudentFkNavigation = _context.students.ToList();

                Invoices invoice = _r.GetById(SID);
                invoice.StudentFk = inv.StudentFk;
                invoice.Status = inv.Status;
                invoice.Description = inv.Description;
                invoice.Rate = inv.Rate;
                invoice.FromAddress = inv.FromAddress;
                invoice.ToAddress = inv.ToAddress;
                invoice.Discount = inv.Discount;
                invoice.DueDate = inv.DueDate;
                invoice.CreatedDate = inv.CreatedDate;
                invoice.Category = inv.Category;
                invoice.InoviceAmount = inv.InoviceAmount;
                invoice.Quantity = inv.Quantity;
                if (invoice != null)
                {
                    _r.Editing(invoice);
                    TempData["MsgS"] = "تم تحديث البيانات بنجاح";
                    return RedirectToAction("Index");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
    
        public IActionResult InvoiceGrid()
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];

            if (TempData["LoginID"] != null)
            {

                TempData["InvoicesGActive"] = "active";
                return View(_context.invoices.Include(a => a.StudentFkNavigation).ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }    
      
        public IActionResult ViewInvoice(int SID,string Name)
        {
            TempData["LoginID"] = Request.Cookies["LoginID"];
            TempData["LoginName"] = Request.Cookies["LoginName"];


            if (TempData["LoginID"] != null)
            {
                if (_r.GetById(SID) != null)
                {
                    TempData["STNAME"] = Name;
                    return View(_r.GetById(SID));
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
