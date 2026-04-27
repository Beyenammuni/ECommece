using ClinicAppointmentHR.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClinicManagement.App.Models
{
    public class UserModel: IdentityUser
    {
        [Required(ErrorMessage = "First Name is required"), StringLength(50)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required"), StringLength(150)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required"), StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required"), StringLength(200)]
        public string Password { get; set; }

        public int? DoctorId { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool IsActive { get; set; }
        public virtual DoctorModel? Doctor { get; set; }

    }
}
