using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Application.DTOs
{
    public class MenuItemDto
    {
        public int PageId { get; set; }
        public string PageName { get; set; } = string.Empty;
        public string PageUrl { get; set; } = string.Empty;
        public int? ParentPageId { get; set; }
        public int DisplayOrder { get; set; }
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanUpdate { get; set; }

        public List<MenuItemDto> Children { get; set; } = new();
    }
}
