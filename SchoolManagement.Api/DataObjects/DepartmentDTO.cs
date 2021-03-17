using SchoolManagement.Core.Entities;
using System;
using System.Collections.Generic;

namespace SchoolManagement.Api.DataObjects
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public virtual ICollection<Course> Courses { get; set; } = Array.Empty<Course>();
        public virtual ICollection<User> Users { get; set; } = Array.Empty<User>();
    }
}
