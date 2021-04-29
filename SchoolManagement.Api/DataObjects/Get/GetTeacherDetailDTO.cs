using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using System;
using System.Collections.Generic;

namespace SchoolManagement.Api.DataObjects.Get
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<GetTeacherDetailDTO>))]
    public class GetTeacherDetailDTO
    {
        public string IdCard { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool? Gender { get; set; }
        public string Birthdate { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? IsHead { get; set; }

        public ICollection<ClassDTO> Classes { get; set; } = Array.Empty<ClassDTO>();
    }
}
