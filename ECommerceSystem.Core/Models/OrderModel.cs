using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommeceSystem.EF.Models
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        public string Status { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Navigation
        [ForeignKey("CustomerId")]
        public UserModel Customer { get; set; }

        public ICollection<OrderItemModel> OrderItems { get; set; }
    }
}
