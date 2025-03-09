﻿using System;
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

            // Convert to UTC to prevent timezone issues
            DateTime startUtc = instance.StartTime.ToUniversalTime();
            DateTime endUtc = endTime.ToUniversalTime();

            Console.WriteLine($"DEBUG: StartTime = {startUtc}, EndTime = {endUtc}");

            if (startUtc >= endUtc)
            {
                return new ValidationResult("End time must be later than start time.");
            }
            return ValidationResult.Success;
        }
    }
}
