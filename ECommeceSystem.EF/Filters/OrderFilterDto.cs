using ECommerceSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommeceSystem.EF.Filters
{

    public class OrderFilterDto
    {
        public OrderStatus? Status { get; set; }

        public int? CustomerId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
