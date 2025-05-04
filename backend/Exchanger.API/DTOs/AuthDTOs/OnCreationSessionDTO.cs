using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.DTOs.AuthDTOs
{
    public class OnCreationSessionDTO
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public CookieOptions AccessTokenCookieOptions { get; set; }

        [Required]
        public CookieOptions RefreshTokenCookieOptions { get; set; }
    }
}
