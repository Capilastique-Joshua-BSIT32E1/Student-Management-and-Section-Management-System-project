using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
    public int SubjectId { get; set; } // Foreign Key

    [ForeignKey("SubjectId")]
    public Subject Subject { get; set; }
}
