using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagement.Api.DataObjects
{
    public class RoleDTO
    {
        [FromRoute]
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
