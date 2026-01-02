using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entities
{
    public class User : BaseEntity
    {
        [Column("UserId")] // maps property Id from BaseEntity to DB column UserId
        public new int Id { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string DisplayName { get; set; } = "";

        [NotMapped]
         public int RoleId { get; set; } // not in user table so often commented but as added NotMapped so uncommented as it may break the rule as table does not have then problem can be occur when through entity framework core save . somewhere read that if table does not have this column then conflict

    }
}
