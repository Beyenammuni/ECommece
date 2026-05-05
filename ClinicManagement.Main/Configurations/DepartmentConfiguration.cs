using ClinicAppointmentHR.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicMagagementSystem.Main.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<DepartmentModel>
    {
        public void Configure(EntityTypeBuilder<DepartmentModel> builder)
        {
            builder.HasIndex(d => d.Name).IsUnique();
            builder.Property(d=>d.Name).IsRequired();
            builder.HasMany(b => b.Doctors).WithOne(b => b.Department)
                .HasForeignKey(b => b.DepartmentId);
        }
    }
}
