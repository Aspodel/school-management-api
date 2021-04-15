using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Api.DataObjects.Create
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<CreateTeacherDTO>))]
    public class CreateTeacherDTO
    {
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        public bool? Gender { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
        public bool? IsHead { get; set; }
    }
}
