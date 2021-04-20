using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagement.Api.DataObjects.Create
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<CreateClassDTO>))]
    public class CreateClassDTO
    {
        [Required]
        public string CourseCode { get; set; } = string.Empty;

        public string TeacherIdCard { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public DayOfWeek Day { get; set; }
        public int StartPeriods { get; set; }
        public int Periods { get; set; }
        public int? Slot { get; set; }
    }
}
