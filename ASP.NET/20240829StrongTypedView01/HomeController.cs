using _20240829_StronTypedView01.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _20240829_StronTypedView01.Controllers
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
            //// Razor ��� obj �ѱ�
            //Employee obj = new Employee()
            //{
            //    EmpID = 100, EmpName = "ȫ�浿", Designation = "�븮", Salary = 4000000
            //};

            var emps = new List<Employee>()
            {
                new Employee { EmpID = 1, EmpName = "ȫ�浿", Designation = "�븮", Salary = 4000000 },
                new Employee { EmpID = 2, EmpName = "�̼���", Designation = "����", Salary = 6000000 },
                new Employee { EmpID = 3, EmpName = "������", Designation = "��", Salary = 8000000 }
            };

            //return View(obj);
            return View(emps);
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
