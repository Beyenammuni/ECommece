using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Core.Models
{

        public abstract class BaseEntity
        {
            public DateTime CreatedAt { get; set; }
                = DateTime.UtcNow;

            public DateTime? UpdatedAt { get; set; }

            public bool IsActive { get; set; } = true;
        }
}
