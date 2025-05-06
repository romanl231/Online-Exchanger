using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.Entities
{
    public class SessionToken
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public string DeviceType {  get; set; }

        [Required]
        public string IpAdress {  get; set; }

        [Required]
        public bool IsRevoked { get; set; }
    }
}
