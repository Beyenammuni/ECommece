using ClinicAppointmentHR.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicMagagementSystem.Main.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<PatientModel>
    {
        public void Configure(EntityTypeBuilder<PatientModel> builder)
        {
            builder.HasIndex(p => p.Phone).IsUnique();
            builder.Property(p => p.Phone).IsRequired().HasMaxLength(15);
        }
    }
}
