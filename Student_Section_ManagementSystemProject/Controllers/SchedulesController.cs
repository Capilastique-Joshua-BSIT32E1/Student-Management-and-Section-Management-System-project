using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Section_ManagementSystemProject.Data;
using Student_Section_ManagementSystemProject.Models;
using System.Linq;

namespace Student_Section_ManagementSystemProject.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.Schedules.Include(s => s.Subject).ToList());

        public IActionResult Create()
        {
            ViewBag.Subjects = _context.Subjects.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Schedules.Add(schedule);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(schedule);
        }

        public IActionResult Delete(int id)
        {
            var schedule = _context.Schedules.Find(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}