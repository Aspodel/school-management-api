﻿using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;

namespace SchoolManagement.Api.DataObjects.Get
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<GetStudentDTO>))]
    public class GetStudentDTO
    {
        public string IdCard { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
