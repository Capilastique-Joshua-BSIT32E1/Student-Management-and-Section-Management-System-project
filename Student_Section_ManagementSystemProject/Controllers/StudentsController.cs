using Microsoft.AspNetCore.Mvc;

namespace Student_Section_ManagementSystemProject.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
