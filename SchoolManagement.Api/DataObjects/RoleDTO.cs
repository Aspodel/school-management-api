using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Services;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Api.DataObjects
{
    [ModelBinder(typeof(MultipleSourcesModelBinder<RoleDTO>))]
    public class RoleDTO : BaseDTO<string>
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
