using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Api.DataObjects.Create
{
    public class CreateUserDTO
    {
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [Required]
        public IList<string> Roles { get; set; } = Array.Empty<string>();

        public int DepartmentId { get; set; }
    }
}
