using System;
using System.Collections.Generic;
using System.Text;

namespace ECommeceSystem.EF.Filters
{
    public class ProductFilterDto
    {
        public string Search { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
