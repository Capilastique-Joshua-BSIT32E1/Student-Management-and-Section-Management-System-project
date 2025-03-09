namespace Student_Section_ManagementSystemProject.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
    }
}