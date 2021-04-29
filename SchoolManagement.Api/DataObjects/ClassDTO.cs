using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using System;

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
    }
}
