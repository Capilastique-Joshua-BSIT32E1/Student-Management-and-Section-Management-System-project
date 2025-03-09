namespace Student_Section_ManagementSystemProject.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
