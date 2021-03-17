using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public class Teacher : User
    {
        public bool? IsHead { get; set; } = false;

        public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
    }
}
