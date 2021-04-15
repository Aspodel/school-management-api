using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public class Student : User
    {
        public virtual ICollection<Class> Classes { get; set; } = new HashSet<Class>();
    }
}
