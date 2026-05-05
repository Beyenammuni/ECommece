using ClinicAppointmentHR.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicMagagementSystem.Main.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<AppointmentModel>
    {
        public void Configure(EntityTypeBuilder<AppointmentModel> builder)
        {
            builder.HasOne(a => a.Patient)
                   .WithMany(p => p.Appointments)
                   .HasForeignKey(a => a.PatientId)
                   .OnDelete(DeleteBehavior.Cascade);   
            builder.ToTable(a => a.HasCheckConstraint("CK_Appointment_AppointmentDate", "AppointmentDate >= GETDATE()"));
            //A doctor cannot have two active appointments at the same date and time.
            builder.ToTable(a => a.HasCheckConstraint("", " "));
        }
    }
}
