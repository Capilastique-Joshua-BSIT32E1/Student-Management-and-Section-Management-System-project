﻿using Microsoft.AspNetCore.Mvc;
using Student_Section_ManagementSystemProject.Data;
using Student_Section_ManagementSystemProject.Models;
using System.Linq;

namespace Student_Section_ManagementSystemProject.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Students.ToList());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                _context.SaveChanges();
                TempData["StudentSuccessMessage"] = "Student successfully added!";
                return RedirectToAction(nameof(Index));
            }
            TempData["StudentErrorMessage"] = "Failed to add student. Please check your inputs.";
            return View(student);
        }

        public IActionResult Edit(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                TempData["StudentErrorMessage"] = "Student not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Update(student);
                _context.SaveChanges();
                TempData["StudentSuccessMessage"] = "Student details successfully updated!";
                return RedirectToAction(nameof(Index));
            }
            TempData["StudentErrorMessage"] = "Failed to update student. Please check your inputs.";
            return View(student);
        }

        public IActionResult Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
                TempData["StudentSuccessMessage"] = "Student successfully deleted!";
            }
            else
            {
                TempData["StudentErrorMessage"] = "Student not found!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
