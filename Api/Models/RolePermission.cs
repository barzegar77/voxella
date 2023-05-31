using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class RolePermission
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }

        public Guid? PermissionId { get; set; }
        [ForeignKey(nameof(PermissionId))]
        public Permission? Permission { get; set; }
    }
}