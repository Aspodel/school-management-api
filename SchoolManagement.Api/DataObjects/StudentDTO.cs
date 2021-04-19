﻿using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Api.DataObjects
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<StudentDTO>))]
    public class StudentDTO
    {
        [FromRoute]
        public string IdCard { get; set; } = string.Empty;

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

        public ICollection<int> Classes { get; set; } = Array.Empty<int>();
    }
}
