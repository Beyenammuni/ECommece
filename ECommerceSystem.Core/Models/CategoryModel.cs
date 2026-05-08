using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommeceSystem.EF.Models
{
    public class CategoryModel
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public ICollection<ProductModel> Products { get; set; }
    }
}
