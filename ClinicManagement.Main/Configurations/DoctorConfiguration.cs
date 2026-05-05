using ClinicAppointmentHR.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicMagagementSystem.Main.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<DoctorModel>
    {
        public void Configure(EntityTypeBuilder<DoctorModel> builder)
        {
            builder.HasIndex(m => m.Email).IsUnique();
           builder.Property(c=>c.Email).HasMaxLength(100).IsRequired();
          
        }
    }
}
