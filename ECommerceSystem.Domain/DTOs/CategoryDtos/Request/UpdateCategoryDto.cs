using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerceSystem.Domain.DTOs.CategoryDtos.Request
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        public string Name { get; set; }
    }
}
