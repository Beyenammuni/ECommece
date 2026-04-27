using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicAppointmentHR.Dtos.PatientDtos
{
    public class PatientDto
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
