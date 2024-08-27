using Microsoft.AspNetCore.Mvc;

namespace _20240827_EmptyQuizForm.Controllers
{
    public class HomeController : Controller
    {
        // 안 적었을 때는 기본으로 GET
        public IActionResult Index()
        {
            return View();
        }

        // Post
        [HttpPost]
        public IActionResult index(int number1, int number2)
        {
            ViewBag.Result = number1 + number2;

            return View();
        }
    }
}
