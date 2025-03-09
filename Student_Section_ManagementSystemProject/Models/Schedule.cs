using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    public class Schedule
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [CustomValidation(typeof(Schedule), nameof(ValidateTime))]
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
            return (instance.StartTime < endTime)
                ? ValidationResult.Success
                : new ValidationResult("End Time must be later than Start Time.");
        }
    }
}
