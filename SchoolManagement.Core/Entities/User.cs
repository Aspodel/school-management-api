using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Core.Entities
{
    public class User : IdentityUser
    {
        //public string Guid { get; protected set; } = null!;
        public string FullName { get; set; } = string.Empty;
        public string IdCard { get; protected set; } = string.Empty;
        public bool? Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; } = string.Empty;

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; } = new HashSet<UserRole>();
    }
}
