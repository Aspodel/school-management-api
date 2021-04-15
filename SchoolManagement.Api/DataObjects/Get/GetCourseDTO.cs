using SchoolManagement.Core.Entities;
using System;
using System.Collections.Generic;

namespace SchoolManagement.Api.DataObjects.Get
{
    public class GetCourseDTO
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }

        public virtual ICollection<Class> Classes { get; set; } = Array.Empty<Class>();
    }
}
