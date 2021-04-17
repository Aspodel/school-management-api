﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public class User : IdentityUser
    {
        public string IdCard { get; set; } = null!;
        public string FullName { get; set; } = string.Empty;
        public bool? Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string? Address { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<UserRole> UserRoles { get; } = new HashSet<UserRole>();
    }
}
