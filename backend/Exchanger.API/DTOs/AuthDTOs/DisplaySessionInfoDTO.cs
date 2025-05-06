using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.DTOs.AuthDTOs
{
    public class DisplaySessionInfoDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public string DeviceType { get; set; }

        [Required]
        public string IpAdress { get; set; }
    }
}
