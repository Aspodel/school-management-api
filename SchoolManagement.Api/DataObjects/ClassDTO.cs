using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using SchoolManagement.Core.Entities;
using System;
using System.Collections.Generic;

namespace SchoolManagement.Api.DataObjects
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<ClassDTO>))]
    public class ClassDTO : BaseDTO
    {
        [FromRoute]
        public string ClassCode { get; set; } = string.Empty;

        public int CourseId { get; set; }
        public string? TeacherId { get; set; }
        public string Room { get; set; } = string.Empty;
        public DayOfWeek Day { get; set; }
        public int StartPeriods { get; set; }
        public int Periods { get; set; }
        public int? Slot { get; set; }
        public int? RestSlot { get; set; }

        public ICollection<Student> Students { get; set; } = Array.Empty<Student>();
    }
}
