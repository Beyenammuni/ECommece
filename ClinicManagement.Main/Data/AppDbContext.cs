using System;
using System.Collections.Generic;
using System.Text;
using ClinicAppointmentHR.Models;
using ClinicManagement.App.Dtos.DoctorDtos;
using ClinicManagement.App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace ClinicAppointmentHR.Data
{
    public class AppDbContext : IdentityDbContext<UserModel>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<PatientModel> Patiens { get; set; }
        public DbSet<DoctorModel> Doctors { get; set; }
        public DbSet<AppointmentModel> Appointments { get; set; }
        public DbSet<PaymentModel> Payments { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<DoctorWorkingHours> DoctorWorkingHours { get; set; }
 

    }
}
