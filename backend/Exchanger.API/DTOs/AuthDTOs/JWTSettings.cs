﻿using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.DTOs.AuthDTOs
{
    public class JWTSettings
    {
        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public string Key {  get; set; } = string.Empty;

        public int AuthTokenValidityInMinutes { get; set; }

        public int RefreshTokenValidityInDays { get; set; }

        public int EmailJWTValidityInDays { get; set; }
    }
}
