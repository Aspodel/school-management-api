using SchoolManagement.Core.Entities;
using System.Collections.Generic;

namespace SchoolManagement.Api.DataObjects.Get
{
    public class GetDepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //public string ShortName { get; set; } = string.Empty;
    }
}
