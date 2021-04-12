using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.Entities;
using System;
using System.Collections.Generic;

namespace SchoolManagement.Api.DataObjects
{
    public class DepartmentDTO
    {
        [FromRoute]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public virtual ICollection<Course> Courses { get; set; } = Array.Empty<Course>();
        public virtual ICollection<StudentDTO> Students { get; set; } = Array.Empty<StudentDTO>();
        public virtual ICollection<Teacher> Teachers { get; set; } = Array.Empty<Teacher>();
    }
}
