using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagementSystem.App.Dtos.AppointmentDtos
{
    public class ScheduletAppointmentDto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
