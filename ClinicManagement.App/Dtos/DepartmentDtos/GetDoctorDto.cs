using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagementSystem.App.Dtos.DepartmentDtos
{
    public class GetDoctorDto
    {
        public int Doctor { get; set; }
        public string DoctorName { get; set; }
        public int DoctorType { get; set; }
    }
}
