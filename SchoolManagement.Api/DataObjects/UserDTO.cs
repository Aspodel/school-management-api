﻿using System;
using System.Collections.Generic;

namespace SchoolManagement.Api.DataObjects
{
    public class UserDTO
    {
        public string IdCard { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; } = string.Empty;

        public ICollection<string> Roles { get; set; } = Array.Empty<string>();
    }
}
