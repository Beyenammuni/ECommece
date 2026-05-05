using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.App.DTOs.ProductDtos.Response
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }

        public bool IsActive { get; set; }
    }
}
