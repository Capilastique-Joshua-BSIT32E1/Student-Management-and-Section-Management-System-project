using Microsoft.AspNetCore.Mvc;
using Student_Section_ManagementSystemProject.Data;
using Student_Section_ManagementSystemProject.Models;
using System.Linq;

namespace Student_Section_ManagementSystemProject.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.Subjects.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Subjects.Add(subject);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        public IActionResult Edit(int id) => View(_context.Subjects.Find(id));

        [HttpPost]
        public IActionResult Edit(Subject subject)
        {
            _context.Subjects.Update(subject);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var subject = _context.Subjects.Find(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
