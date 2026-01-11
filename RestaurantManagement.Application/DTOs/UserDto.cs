
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantManagement.Application.DTOs
{
    public class UserDto
    {
        
        public int Id { get; set; }

       // public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public bool IsActive { get; set; } 
       
        public int RoleId { get; set; }
    }
}
