using Microsoft.AspNetCore.Mvc;

namespace _20240828_ViewDataEmpty.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            TempData["person1"] = "홍길동";
            ViewData["person2"] = "이순신";

            return View();
        }

        public IActionResult UseViewBag()
        {
            ViewBag.data1 = "data1";
            ViewBag.data2 = 100;
            ViewBag.data3 = DateTime.Now.ToShortDateString();

            string[] arr = { "사과", "배", "딸기" };
            ViewBag.data4 = arr;

            ViewBag.data5 = new List<string>()
            {
                "축구", "야구", "농구"
            };

            return View();
        }

        public IActionResult UseTempData()
        {
            ViewData["data1"] = "홍길동";
            ViewBag.data2 = "이순신";
            TempData["data3"] = "강감찬";
            TempData["data4"] = new List<string>() { "야구", "농구", "축구" };

            // TempData는 Keep이 가능 = 임시로 저장(기억)
            TempData.Keep("data3");

            return View();
        }

        public IActionResult About()
        {
            TempData.Keep("data3");
            return View();
        }
    }
}
