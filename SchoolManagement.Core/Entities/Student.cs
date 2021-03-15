using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Core.Entities
{
    public class Student : User
    {
        public virtual ICollection<StudentCourse> EnrolledCourses { get; set; } = new HashSet<StudentCourse>();
    }
}
