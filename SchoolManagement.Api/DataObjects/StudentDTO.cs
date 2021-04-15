﻿using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using SchoolManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

        //public ICollection<string> Roles { get; set; } = Array.Empty<string>();
        public ICollection<Class> Classes { get; set; } = Array.Empty<Class>();
    }
}
