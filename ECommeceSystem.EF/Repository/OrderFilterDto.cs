using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.DTOs.OrderDtos.Response
{

    public class OrderFilterDto
    {
        public OrderStatus? Status { get; set; }

        public int? CustomerId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
