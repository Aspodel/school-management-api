using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using SchoolManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Api.DataObjects
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<CourseDTO>))]
    public class CourseDTO : BaseDTO
    {
        [FromRoute]
        public string CourseCode { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int Credits { get; set; }

        public ICollection<Class> Classes { get; set; } = Array.Empty<Class>();
    }
}
