using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Domain.Entities.RoleModel
{
    public class RolePermission
    {
        [Key]
        public int? Id { get; set; }

        [ForeignKey("Role")]
        public int? RoleId { get; set; }
        public Role? Role { get; set; }

        [ForeignKey("Permission")]
        public int? PermissionId { get; set; }
        public Permission? Permission { get; set; }

        public RolePermission()
        {
            this.Id = null;
            this.RoleId = null;
            this.PermissionId = null;
            this.Role = null;
            this.Permission = null;
        }
    }
}
