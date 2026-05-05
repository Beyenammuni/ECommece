using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Core.Result
{
    public enum FailureType
    {
        none = 0,
        NotFound = 1,
        BadRequest = 2,
        Unauthorized = 3,
        Forbidden = 4,
        Conflict = 5,
        InternalError = 6
    }
}
