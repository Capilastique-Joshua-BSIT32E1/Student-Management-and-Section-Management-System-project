﻿using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Student_Section_ManagementSystemProject.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SubjectId { get; set; }
        public virtual Subject? Subject { get; set; }  // Ensure Subject is referenced

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        // 🛠 FIX: Add navigation property for Enrollments
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
