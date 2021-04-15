using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Api.DataObjects.Create
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<CreateCourseDTO>))]
    public class CreateCourseDTO
    {
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Credits { get; set; }
    }
}
