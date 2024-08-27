using Microsoft.AspNetCore.Mvc;

namespace _20240827_MVCEmptyApp01.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Help()
        {
            return View();
        }
    }
}
