using Microsoft.AspNetCore.Mvc;

namespace _20240827_MapControllerQuiz.Controllers
{
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
        public IActionResult Greet()
        {
            return View();
        }

        [Route("[action]")]
        public IActionResult About()
        {
            return View();
        }

        [Route("[action]/{id?}")]
        public int Help(int? id)
        {
            return id ?? 50;
        }
    }
}
