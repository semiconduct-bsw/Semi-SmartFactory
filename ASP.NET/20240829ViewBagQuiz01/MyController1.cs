using Microsoft.AspNetCore.Mvc;

namespace _20240829_ViewBagQuiz01.Controllers
{
    public class MyController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Output()
        {
            ViewBag.data1 = new List<string>()
            {
                "C#프로그래밍", "Java 정복", "HTML5", "CSS 하루만에하기"
            };

            string[] foods = { "된장국", "김치찌개", "소금빵", "두루치기" };
            TempData["temp01"] = foods;
            return View();
        }
    }
}
