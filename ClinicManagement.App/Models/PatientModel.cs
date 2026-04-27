using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClinicAppointmentHR.Models
{
    public class PatientModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; } 
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public ICollection<AppointmentModel> Appointments { get; set; }
    }
}
