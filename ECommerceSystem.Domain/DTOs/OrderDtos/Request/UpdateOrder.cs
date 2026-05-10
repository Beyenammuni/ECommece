using ECommerceSystem.Domain.DTOs.OrderDtos.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ECommerceSystem.Domain.DTOs.OrderDtos.Request
{
    public class UpdateOrder
    {
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
    }
}
