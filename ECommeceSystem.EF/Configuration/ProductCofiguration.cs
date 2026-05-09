using ECommeceSystem.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommeceSystem.EF.Configuration
{
    public class ProductCofiguration:IEntityTypeConfiguration<ProductModel>
    {
        public void Configure(EntityTypeBuilder<ProductModel> builder)
        {
            builder.HasQueryFilter(c => c.IsActive);
            builder.ToTable(x=>x.HasCheckConstraint("Check_StockQuantity_NonNegative", "[StockQuantity] >= 0"));
            builder.ToTable(x => x.HasCheckConstraint("Check_Price", "[Price] > 0"));
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.HasOne(x => x.Category).WithMany(x => x.Products)
             .HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
