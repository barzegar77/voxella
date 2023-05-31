using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Permission
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}