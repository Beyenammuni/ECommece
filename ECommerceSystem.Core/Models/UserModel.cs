using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommeceSystem.EF.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; } // Admin | Customer

        // Navigation
        public ICollection<CartItemModel> CartItems { get; set; }
        public ICollection<OrderModel> Orders { get; set; }
    }

}
