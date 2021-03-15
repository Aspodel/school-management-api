using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Core.Entities
{
    public class Teacher : User
    {
        public bool? IsHead { get; set; } = false;

        public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
    }
}
