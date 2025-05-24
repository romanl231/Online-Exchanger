using CloudinaryDotNet.Actions;
using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.Entities
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

        public bool IsEmailVerified { get; set; } = false;
        public string EmailVerificationCode { get; set; } = string.Empty;

        public ICollection<SessionToken> SessionTokens { get; set; } = new List<SessionToken>();
        public ICollection<Listing> Listings { get; set; } = new List<Listing>();
    }
}
