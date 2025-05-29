using Microsoft.AspNetCore.Mvc;

namespace WebApplication39.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About() 
        { 
            return View(); 
        }
    }
}
