using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Interfaces.Security
{
    public interface IRightResolver
    {
        /// <summary>          
        /// Contract to resolve required permission claim for a request.
        /// </summary>
       
            string? ResolveRequiredPermission(HttpContext httpContext);
        
    }
}
