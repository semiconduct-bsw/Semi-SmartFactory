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

                // ���� ��ġ, �ڴ� Controller
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || context.Persons == null) { return NotFound(); }

            // �������� ���
            var personData = context.Persons.Find(id);
            if (personData == null) { return NotFound(); }

            return View();
        }

        [HttpPost] // ������ ����
        public IActionResult Edit(int? id, Person person)
        {
            if (id != person.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                context.Update(person);
                // SaveChangesAsync = �񵿱� ����϶� ���
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
            // ID ���� �о����
            var personData = context.Persons.FirstOrDefault((x => x.ID == id));

            if (personData == null) { return NotFound(); }
            return View(personData);
        }

        // ������ Post���� �۾�
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
