using _20240903_WebAddressBookExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace _20240903_WebAddressBookExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly PersonDbContext context;

        public HomeController(PersonDbContext _context, ILogger<HomeController> _logger)
        {
            _logger = logger;
            context = _context;
        }

        public IActionResult Index()
        {
            var persons = context.Persons.ToList();
            return View(persons);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                context.Persons.Add(person);
                context.SaveChanges();

                // 앞은 위치, 뒤는 Controller
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || context.Persons == null) { return NotFound(); }

            // 정상적인 경우
            var personData = context.Persons.Find(id);
            if (personData == null) { return NotFound(); }

            return View();
        }

        [HttpPost] // 동기방식 제작
        public IActionResult Edit(int? id, Person person)
        {
            if (id != person.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                context.Update(person);
                // SaveChangesAsync = 비동기 방식일때 사용
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(person);

        }

        public IActionResult Details(int? id)
        {
            if (id == null || context.Persons == null)
            {
                return NotFound();
            }

            var personData = context.Persons.FirstOrDefault(x => x.ID == id);

            if (personData == null)
            {
                return NotFound();
            }

            return View(personData);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || context.Persons == null)
            {
                return NotFound();
            }
            // ID 값을 읽어오기
            var personData = context.Persons.FirstOrDefault((x => x.ID == id));

            if (personData == null) { return NotFound(); }
            return View(personData);
        }

        // 삭제는 Post에서 작업
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? id)
        {
            var personData = context.Persons.Find(id);
            if (personData != null)
            {
                context.Persons.Remove(personData);
            }
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
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
