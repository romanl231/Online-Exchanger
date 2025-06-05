using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.Entities
{
    public class SessionToken
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = new User();

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public string DeviceType {  get; set; } = string.Empty;

        [Required]
        public string IpAdress {  get; set; } = string.Empty;

        [Required]
        public bool IsRevoked { get; set; }
    }
}
