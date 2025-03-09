using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student_Section_ManagementSystemProject.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }


        [Required]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; } // Foreign key

        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        // ✅ FIX: Custom validation for EndTime
        public static ValidationResult ValidateTime(DateTime endTime, ValidationContext context)
        {
            var instance = (Schedule)context.ObjectInstance;

            if (instance.StartTime == default || endTime == default)
            {
                return new ValidationResult("Invalid time values.");
            }

            // ✅ Fix: If endTime is earlier than startTime, assume it's the next day
            if (instance.StartTime.TimeOfDay >= endTime.TimeOfDay)
            {
                if (endTime.Hour < instance.StartTime.Hour) // Next-day handling
                {
                    endTime = endTime.AddDays(1);
                }
                else
                {
                    return new ValidationResult("End time must be later than start time.");
                }
            }

            return ValidationResult.Success;
        }

    }
}
