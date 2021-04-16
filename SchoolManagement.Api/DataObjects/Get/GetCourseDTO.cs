using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;

namespace SchoolManagement.Api.DataObjects.Get
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<GetCourseDTO>))]
    public class GetCourseDTO
    {
        public string CourseCode { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int Classes { get; set; }
    }
}
