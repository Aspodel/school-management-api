using System;

namespace SchoolManagement.Api.DataObjects
{
    public class UserDTO
    {
        public string FullName { get; set; }
        public string IdCard { get; set; }
        public bool Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
