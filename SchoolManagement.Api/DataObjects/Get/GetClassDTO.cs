using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using System;

namespace SchoolManagement.Api.DataObjects.Get
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<GetClassDTO>))]
    public class GetClassDTO : BaseDTO
    {
        public string ClassCode { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string? Teacher { get; set; }
        public string Room { get; set; } = string.Empty;
        public DayOfWeek Day { get; set; }
        public int StartPeriods { get; set; }
        public int Periods { get; set; }
        public int? Slot { get; set; }
        public int? RestSlot { get; set; }
    }
}
