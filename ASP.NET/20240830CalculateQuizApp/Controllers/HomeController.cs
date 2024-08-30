using Microsoft.AspNetCore.Mvc;

namespace _20240830_CalculateQuizApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(int number1, int number2)
        {
            ViewBag.Result1 = number1 + number2;
            ViewBag.Result2 = number1 - number2;
            ViewBag.Result3 = number1 * number2;
            ViewBag.Result4 = number2 != 0 ? (double)number1 / (double)number2 : double.NaN;

            return View();
        }
    }
}
