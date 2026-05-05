using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagement.App.Dtos.BonusDtos
{
    public class PagenationDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
