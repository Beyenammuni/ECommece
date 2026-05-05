using ClinicManagementSystem.App.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicAppointmentHR.Dtos.DoctorDto
{
    public class DoctorDto
    {
        public string FullName { get; set; }
        public SpecialityEnum Specialty { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
    }
}
