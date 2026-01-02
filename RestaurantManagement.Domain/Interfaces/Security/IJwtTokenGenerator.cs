using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Interfaces.Security
{
    public interface IJwtTokenGenerator
    {
        string Generate(int userId, IEnumerable<string> permissions, IEnumerable<string>? roles = null);
    }
}
