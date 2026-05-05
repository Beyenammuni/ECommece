using ClinicManagementSystem.App.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClinicAppointmentHR.Models
{
    public class DoctorModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public SpecialityEnum Specialty { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public string ProfileImageUrl { get; set; }
        public DepartmentModel Department { get; set; } 
        public ICollection<AppointmentModel> Appointments { get; set; }

    }
}
