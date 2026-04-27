using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicAppointmentHR.Dtos.AppointmentDtos
{
    public class AppointmentDto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public string VisitType { get; set; }
        public string Notes { get; set; }
    }
}
