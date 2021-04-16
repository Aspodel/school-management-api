using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagement.Api.DataObjects.Get
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<GetClassDTO>))]
    public class GetClassDTO
    {
        public string ClassCode { get; set; } = string.Empty;
        public string Teacher { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public DayOfWeek Day { get; set; }
        public int StartPeriods { get; set; }
        public int Periods { get; set; }
        public int? Slot { get; set; }
        public int? RestSlot { get; set; }
    }
}
