using _20240828_ModelDataMVC.Models;
using _20240828_ModelDataMVC.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _20240828_ModelDataMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StudentRepository _studentRepository = null;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _studentRepository = new StudentRepository();
        }

        public List<StudentModel> getAllStudents()
        {
            return _studentRepository.getAllStudents();
        }
        public StudentModel getById(int id)
        {
            return _studentRepository.getStudentById(id);
        }

        public IActionResult Index()
        {
            var students = new List<StudentModel>
            {
                new StudentModel { ID = 1, Name = "홍길동", HP = "010-1111-1111", Major="응용화학" },
                new StudentModel { ID = 2, Name = "이순신", HP = "010-2222-2222", Major="산업공학" },
                new StudentModel { ID = 3, Name = "강감찬", HP = "010-3333-3333", Major="컴퓨터공학" }
            };

            ViewData["myStudents"] = students;
            return View();
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
