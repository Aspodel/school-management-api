using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public class Role : IdentityRole
    {
        //public const string Admin = "admin";
        //public const string Student = "student";
        //public const string Teacher = "teacher";

        public virtual ICollection<UserRole> UserRoles { get; } = new HashSet<UserRole>();
    }
}
