using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;

namespace SchoolManagement.Api.DataObjects
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<DepartmentDTO>))]
    public class DepartmentDTO : BaseDTO
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
    }
}
