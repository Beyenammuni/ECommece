using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommeceSystem.EF.Models
{
    public class CartItemModel
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("CustomerId")]
        public UserModel Customer { get; set; }

        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }
    }
}
