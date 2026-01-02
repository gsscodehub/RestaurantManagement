using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entities
{
    public class Role : BaseEntity
    {
        [Key]
        [Column("RoleId")] // Map BaseEntity.Id to PageId column
        public new int Id { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
