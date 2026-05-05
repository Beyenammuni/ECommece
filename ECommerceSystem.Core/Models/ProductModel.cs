using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ECommeceSystem.EF.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }

        public bool IsActive { get; set; }

        public CategoryModel Category { get; set; }

        public ICollection<CartItemModel> CartItems { get; set; }
        public ICollection<OrderItemModel> OrderItems { get; set; }
    }
}
