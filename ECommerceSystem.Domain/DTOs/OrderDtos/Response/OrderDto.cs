using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ECommerceSystem.Domain.DTOs.OrderDtos.Response
{
    public class OrderDto
    {
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
    }
}
