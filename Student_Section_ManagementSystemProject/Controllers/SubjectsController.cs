using Microsoft.AspNetCore.Mvc;

namespace Student_Section_ManagementSystemProject.Controllers
{
    public class SubjectsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
