using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagement.App.Dtos.RegistrationDtos.Request
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string? Role { get; set; }
        public int? DoctorId { get; set; }
        public DateTime CreateAt  { get; set; }

    }
}
