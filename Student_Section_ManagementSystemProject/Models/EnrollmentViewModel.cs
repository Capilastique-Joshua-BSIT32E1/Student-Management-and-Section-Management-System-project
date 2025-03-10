using System.Collections.Generic;
using Student_Section_ManagementSystemProject.Models;

namespace Student_Section_ManagementSystemProject.ViewModels
{
    public class EnrollmentViewModel
    {
        public List<Student> StudentsList { get; set; } = new List<Student>();
        public List<Schedule> SchedulesList { get; set; } = new List<Schedule>();
    }
}
