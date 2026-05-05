using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommeceSystem.EF.Models
{
    public class OrderItemModel
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Navigation
        [ForeignKey("OrderId")]
        public OrderModel Order { get; set; }

        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }
    }
}
