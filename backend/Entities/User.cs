using System.ComponentModel.DataAnnotations;

namespace backend.Entities
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(500)]
        public string AvatarUrl { get; set; }
    }
}
