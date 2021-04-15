using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public partial class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;

        public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
        public virtual ICollection<Teacher> Teachers { get; set; } = new HashSet<Teacher>();
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
