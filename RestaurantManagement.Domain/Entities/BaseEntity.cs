using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }

        public DateTime CreatedOn { get; protected set; }
        public int? CreatedBy { get; protected set; }

        public DateTime? UpdatedOn { get; protected set; }
        public int? UpdatedBy { get; protected set; }

        public bool IsActive { get; protected set; } = true;

        protected BaseEntity()
        {
            CreatedOn = DateTime.UtcNow;
        }

        public void MarkCreated(int userId)
        {
            CreatedOn = DateTime.UtcNow;
            CreatedBy = userId;
            IsActive = true;
        }
        public void MarkUpdated(int userId)
        {
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = userId;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
