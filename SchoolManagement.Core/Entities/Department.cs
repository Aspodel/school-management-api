using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public partial class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;

        public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
