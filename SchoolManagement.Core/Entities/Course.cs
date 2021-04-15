using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public partial class Course
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }

        public virtual Department? Department { get; set; }
        public virtual ICollection<Class> Classes { get; set; } = new HashSet<Class>();
    }
}
