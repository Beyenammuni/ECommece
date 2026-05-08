using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerceSystem.Domain.DTOs.ProductDtos.Request
{
    public class UpdateStockDto
    {
        [Required]
        public int StockQuantity { get; set; }
    }
}
