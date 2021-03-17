using SchoolManagement.Core.Entities;
using System;
using System.Collections.Generic;

namespace SchoolManagement.Api.DataObjects.Get
{
    public class GetCourseDTO
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string TeacherId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int Slot { get; set; }
        public int RestSlot { get; set; }
        public DayOfWeek Day { get; set; }
        public int StartPeriods { get; set; }
        public int Periods { get; set; }
        public string Room { get; set; } = string.Empty;

        public virtual ICollection<StudentCourse> JoinedStudent { get; set; } = Array.Empty<StudentCourse>();
    }
}
