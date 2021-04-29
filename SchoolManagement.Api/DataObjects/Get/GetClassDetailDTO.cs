using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using System;
using System.Collections.Generic;

namespace SchoolManagement.Api.DataObjects.Get
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<GetClassDetailDTO>))]
    public class GetClassDetailDTO
    {
        public string ClassCode { get; set; } = string.Empty;
        public CourseDTO Course { get; set; } = null!;
        public DepartmentDTO Department { get; set; } = null!;
        public GetTeacherDTO Teacher { get; set; } = null!;
        public string Room { get; set; } = string.Empty;
        public DayOfWeek Day { get; set; }
        public int StartPeriods { get; set; }
        public int Periods { get; set; }
        public int? Slot { get; set; }
        public int? RestSlot { get; set; }
        
        public ICollection<GetStudentDTO> Students { get; set; } = Array.Empty<GetStudentDTO>();
    }

}
