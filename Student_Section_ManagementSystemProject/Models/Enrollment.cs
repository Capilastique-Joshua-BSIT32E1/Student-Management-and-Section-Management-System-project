using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Student_Section_ManagementSystemProject.Models
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student? Student { get; set; }

        [Required]
        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }
        public virtual Schedule? Schedule { get; set; }

        // ✅ New properties for listing available students and schedules
        [NotMapped]
        public List<Student> StudentsList { get; set; } = new List<Student>();

        [NotMapped]
        public List<Schedule> SchedulesList { get; set; } = new List<Schedule>();
    }
}
