using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using System;

namespace SchoolManagement.Api.DataObjects.Get
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<GetStudentDTO>))]
    public class GetStudentDTO
    {
        public string IdCard { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool? Gender { get; set; }
        public string Birthdate { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
