using Microsoft.AspNetCore.Mvc;
using System;

namespace SchoolManagement.Api.DataObjects
{
    public class CourseDTO
    {
        [FromRoute]
        public int Id { get; set; }
        public string? TeacherId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int? Slot { get; set; }
        public int? RestSlot { get; set; }
        public DayOfWeek? Day { get; set; }
        public int? StartPeriods { get; set; }
        public int? Periods { get; set; }
        public string Room { get; set; } = string.Empty;

    }
}
