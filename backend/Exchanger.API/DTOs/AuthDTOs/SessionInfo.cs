﻿using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.DTOs.AuthDTOs
{
    public class SessionInfo
    {
        [Required]
        public string DeviceType { get; set; } = string.Empty;
        [Required]
        public string IpAdress { get; set; } = string.Empty;
    }
}
