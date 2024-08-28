using Microsoft.AspNetCore.Mvc;

namespace _20240828_EmptyQuizApp01.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyPage()
        {
            return View();

        }

        [HttpGet]
        public IActionResult InputQuiz()
        {
            return View();
        }
        [HttpPost]
        public IActionResult OutputQuiz(int number)
        {
            // 받은 number를 다시 넘겨주는 역할
            ViewBag.Result = number;
            return View();
        }
    }
}
