using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public class Teacher : User
    {
        //public int DepartmentId { get; set; }
        public bool? IsHead { get; set; } = false;

        //public Department? Department { get; set; }
        public virtual ICollection<Class> Classes { get; set; } = new HashSet<Class>();
    }
}
