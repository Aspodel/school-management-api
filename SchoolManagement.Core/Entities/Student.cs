using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public class Student : User
    {
        public virtual ICollection<StudentCourse> EnrolledCourses { get; set; } = new HashSet<StudentCourse>();
    }
}
