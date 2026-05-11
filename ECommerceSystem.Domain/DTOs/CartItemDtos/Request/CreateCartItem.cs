using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerceSystem.Domain.DTOs.CartItemDtos.Request
{
    public class CreateCartItem
    {


        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
