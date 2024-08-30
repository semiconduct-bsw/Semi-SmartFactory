using _20240830_ModelBindingApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _20240830_ModelBindingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Index.cshtml의 버튼에서 처리한 Post와의 연계
        // 아래 return한 값은 문자열로 출력됨
        [HttpPost]
        public string Index(Student st)
        {
            return "Id:" + st.ID + " Name:" + st.Name + " Hp:" + st.HP;
        }

        public string Details(int id, string name)
        {
            return "Id is: " + id + " Name is: " + name;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
