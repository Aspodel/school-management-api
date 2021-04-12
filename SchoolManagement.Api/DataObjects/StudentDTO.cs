using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagement.Api.DataObjects
{
    public class StudentDTO
    {
        public string IdCard { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public bool Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
