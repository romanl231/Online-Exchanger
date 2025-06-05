using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.DTOs.AuthDTOs
{
    public class OnCreationSessionDTO
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;

        [Required]
        public CookieOptions AccessTokenCookieOptions { get; set; } = new CookieOptions();

        [Required]
        public CookieOptions RefreshTokenCookieOptions { get; set; } = new CookieOptions();
    }
}
