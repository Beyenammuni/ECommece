using ECommeceSystem.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommeceSystem.EF.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemModel>
    {
        public void Configure(EntityTypeBuilder<OrderItemModel> builder)
        {
            builder.HasOne(x=>x.Order).WithMany(x => x.OrderItems)
             .HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
             builder.HasOne(x => x.Product).WithMany(x => x.OrderItems)
             .HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
