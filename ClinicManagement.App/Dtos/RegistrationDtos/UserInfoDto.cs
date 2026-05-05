using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagement.App.Dtos.RegistrationDtos
{
    public class UserInfoDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public bool IsActive { get; set; }
    }
}
