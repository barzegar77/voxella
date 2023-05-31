using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? EmailConfirmationCode { get; set; }
        public DateTime? EmailConfirmationSentDateTime { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsLocked { get; set; } = false;


        public ICollection<UserRole>? UserRoles { get; set; }
    }
}