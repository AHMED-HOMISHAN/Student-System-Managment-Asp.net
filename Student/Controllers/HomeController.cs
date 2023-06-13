using Microsoft.AspNetCore.Mvc;
using Student.Models;
using System.Diagnostics;

namespace Student.Controllers
{
    public class HomeController : Controller
    {
        private readonly stdDBContext _context;
        private readonly ILogger<HomeController> _logger;

     

        public HomeController(ILogger<HomeController> logger,stdDBContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {
            try
            {
                if(Request.Cookies["LoginID"]!=null )
                { 
                    return RedirectToAction("Index", "Student"); 
                }
                 else {
                    return View(); 
                }
            }
            catch (Exception)
            {
                return View();
            }
        } 
        [HttpPost]
        public IActionResult Index(Students _loginModel)
        {
            try
            {
                if (_loginModel!=null)
                {
                    Students status =  _context.students.Where(m => m.Email == _loginModel.Email && m.Password == _loginModel.Password).FirstOrDefault();
                    if (status == null)
                    {
                        ViewBag.LoginStatus = 0;
                    }
                    else
                    {
                        
                        Students l = _context.students.Find(status.Id);
                        Response.Cookies.Append("LoginName", l.Name);
                        Response.Cookies.Append("LoginID", l.Id.ToString());
                        TempData["LoginID"] = Request.Cookies["LoginID"];
                        return RedirectToAction("Index", "Student");
                    }


                }
            }
            catch (Exception)
            {
                return View();
            }

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Logout()
        {
            Response.Cookies.Delete("LoginID");
            return RedirectToAction("Index", "Home");
        }
    }
}
