using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entities
{
    public class Page : BaseEntity
    {
        [Key]
        [Column("PageId")] // Map BaseEntity.Id to PageId column
        public new int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string PageName { get; set; }
        [Required]
        [StringLength(256)]
        public string PageUrl { get; set; }
        public int? ParentPageId { get; set; }
        public int DisplayOrder { get; set; } = 0; 
        [StringLength(256)] 
        public string Description { get; set; }
        [StringLength(100)]
        public string Icon { get; set; }

        // Navigation property for self-referencing parent
        [ForeignKey("ParentPageId")] 
        public virtual Page ParentPage { get; set; } 
        // ---------------- SP-only flags (not part of table) ----------------
        [NotMapped]
        public bool CanView { get; set; } 
        [NotMapped] 
        public bool CanCreate { get; set; } 
        [NotMapped] 
        public bool CanEdit { get; set; } 
        [NotMapped] 
        public bool CanDelete { get; set; }
        [NotMapped]
        public bool CanUpdate { get; set; }
    }
}
