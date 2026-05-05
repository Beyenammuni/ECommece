using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagementSystem.App.Dtos.AppointmentDtos
{
    public class getAppointmentByDotorAndDateDto
    {
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
