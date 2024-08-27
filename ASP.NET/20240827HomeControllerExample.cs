using Microsoft.AspNetCore.Mvc;

namespace _20240827_MVCEmptyApp02.Controllers
{
		[Route("[Controller]")]
    public class HomeController : Controller
    {
		    [Route("~/")]
				[Route("/Home")]
				[Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("[action]")]
        public IActionResult About()
        {
            return View();
        }
        public int Help(int? id)
        {
		        return id?? 100;
        }
    }
}
