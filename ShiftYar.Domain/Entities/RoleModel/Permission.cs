using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Domain.Entities.RoleModel
{
    public class Permission
    {
        [Key]
        public int? Id { get; set; }
        public string? Name { get; set; } // مثلا: ViewUsers, EditHospital, etc.

        public ICollection<RolePermission>? RolePermissions { get; set; }

        public Permission()
        {
            this.Id = null;
            this.Name = null;
            this.RolePermissions = new List<RolePermission>();
        }
    }
}
