using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public class Student : User
    {
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public virtual ICollection<Class> Classes { get; set; } = new HashSet<Class>();
    }
}
