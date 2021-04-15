using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using SchoolManagement.Core.Entities;
using System;
using System.Collections.Generic;

namespace SchoolManagement.Api.DataObjects
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<DepartmentDTO>))]
    public class DepartmentDTO : BaseDTO
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;

        public ICollection<Course> Courses { get; set; } = Array.Empty<Course>();
        public ICollection<Student> Students { get; set; } = Array.Empty<Student>();
        public ICollection<Teacher> Teachers { get; set; } = Array.Empty<Teacher>();
    }
}
