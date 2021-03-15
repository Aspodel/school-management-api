using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string? TeacherId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int? Slot { get; set; }
        public int? RestSlot { get; set; }
        public DayOfWeek? Day { get; set; }
        public int? StartPeriods { get; set; }
        public int? Periods { get; set; }
        public string Room { get; set; } = string.Empty;

        public virtual Teacher? Teacher { get; set; }
        public virtual Department? Department { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new HashSet<StudentCourse>();
    }
}
