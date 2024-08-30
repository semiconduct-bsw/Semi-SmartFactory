using Microsoft.AspNetCore.Mvc;

namespace _20240830_LayoutHeaderFooter.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
