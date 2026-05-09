using ECommeceSystem.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommeceSystem.EF.Configuration
{
    public class CartItemConfigration : IEntityTypeConfiguration<CartItemModel>
    {
        public void Configure(EntityTypeBuilder<CartItemModel> builder)
        {
            builder.HasAlternateKey(x => new { x.CustomerId, x.ProductId });
             builder.HasOne(x => x.Customer).WithMany(x => x.CartItems)
             .HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
             builder.HasOne(x => x.Product).WithMany(x => x.CartItems)
             .HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
