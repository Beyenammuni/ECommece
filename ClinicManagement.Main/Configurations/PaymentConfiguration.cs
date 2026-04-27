using ClinicAppointmentHR.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicMagagementSystem.Main.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<PaymentModel>
    {
        public void Configure(EntityTypeBuilder<PaymentModel> builder)
        {
            builder.ToTable(a=> a.HasCheckConstraint("Amount_uperthenZero", "Amount >= 0"));
        }
    }
}
