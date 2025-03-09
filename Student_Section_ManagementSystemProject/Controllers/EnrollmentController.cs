using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Section_ManagementSystemProject.Data;
using Student_Section_ManagementSystemProject.Models;
using System.Linq;

namespace Student_Section_ManagementSystemProject.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var schedules = _context.Schedules.Include(s => s.Subject).Include(s => s.EnrolledStudents).ToList();
            return View(schedules);
        }

        public IActionResult Enroll(int scheduleId)
        {
            ViewBag.Schedule = _context.Schedules.Include(s => s.Subject).FirstOrDefault(s => s.Id == scheduleId);
            ViewBag.Students = _context.Students.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Enroll(int scheduleId, int studentId)
        {
            var schedule = _context.Schedules.Include(s => s.EnrolledStudents).FirstOrDefault(s => s.Id == scheduleId);
            var student = _context.Students.Find(studentId);

            if (schedule != null && student != null && !schedule.EnrolledStudents.Contains(student))
            {
                schedule.EnrolledStudents.Add(student);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveEnrollment(int scheduleId, int studentId)
        {
            var schedule = _context.Schedules.Include(s => s.EnrolledStudents).FirstOrDefault(s => s.Id == scheduleId);
            var student = schedule?.EnrolledStudents.FirstOrDefault(s => s.Id == studentId);

            if (schedule != null && student != null)
            {
                schedule.EnrolledStudents.Remove(student);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}