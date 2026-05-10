using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.DTOs.OrderDtos.Response
{
    public enum OrderStatus
    {
        None = 0,
        Pending = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5
    }
}
