using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public class User : IdentityUser
    {
        public string IdCard { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public bool? Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        public Department? Department { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; } = new HashSet<UserRole>();
    }
}
