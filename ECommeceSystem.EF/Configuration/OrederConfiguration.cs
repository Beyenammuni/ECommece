using ECommeceSystem.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommeceSystem.EF.Configuration
{
    public class OrederConfiguration: IEntityTypeConfiguration<OrderModel>
    {
        public void Configure(EntityTypeBuilder<OrderModel> builder)
        {
            builder.Property(x => x.OrderDate).IsRequired();
            builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
            builder.HasOne(x => x.Customer).WithMany(x => x.Orders)
             .HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
