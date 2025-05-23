using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.DTOs.AuthDTOs
{
    public class JWTSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Key {  get; set; }

        public int AuthTokenValidityInMinutes { get; set; }

        public int RefreshTokenValidityInDays { get; set; }
    }
}
