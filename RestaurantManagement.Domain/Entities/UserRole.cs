using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        [Key]
        [Column("UserRoleId")]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        [NotMapped]
        public string RoleName { get; set; } = null!;
       
    }
}
