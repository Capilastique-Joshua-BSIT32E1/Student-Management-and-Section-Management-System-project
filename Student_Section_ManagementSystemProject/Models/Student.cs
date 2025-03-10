using System.ComponentModel.DataAnnotations;

namespace Student_Section_ManagementSystemProject.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        // ✅ Foreign Key for Enrollment
        public int? ScheduleId { get; set; }
        public virtual Schedule? Schedule { get; set; }
    }
}
