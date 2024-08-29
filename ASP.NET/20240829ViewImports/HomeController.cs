using _20240829_ViewImportsTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _20240829_ViewImportsTest.Controllers
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
            List<Student> students = new List<Student>();

            Student st1 = new Student();
            st1.ID = 1; st1.Name = "홍길동"; st1.HP = "010-1111-1111";
            students.Add(st1);
            Student st2 = new Student();
            st1.ID = 2; st1.Name = "이순신"; st1.HP = "010-2222-2222";
            students.Add(st2);
            Student st3 = new Student();
            st1.ID = 3; st1.Name = "강감찬"; st1.HP = "010-3333-3333";
            students.Add(st3);

            return View(students);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
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
