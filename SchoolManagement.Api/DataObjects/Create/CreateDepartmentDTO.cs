using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;

namespace SchoolManagement.Api.DataObjects.Create
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<CreateDepartmentDTO>))]
    public class CreateDepartmentDTO
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
    }
}
