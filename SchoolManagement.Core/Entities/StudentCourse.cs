namespace SchoolManagement.Core.Entities
{
    public class StudentCourse
    {
        public string StudentId { get; set; } = string.Empty;
        public virtual Student? Student { get; set; }

        public int CourseId { get; set; }
        public virtual Course? Course { get; set; }
    }
}
