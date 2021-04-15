using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public class Teacher : User
    {
        public bool? IsHead { get; set; } = false;

        public virtual ICollection<Class> Classes { get; set; } = new HashSet<Class>();
    }
}
