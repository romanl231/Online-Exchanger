﻿using Exchanger.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.DTOs.ListingDTOs
{
    public class ListingParams
    {
        public decimal MaxValue { get; set; } = 999999999m;
        public decimal MinValue { get; set; } = 0m;
        public List<Category> Categories { get; set; } = new List<Category>();

        [Required]
        public PaginationDTO Pagination { get; set; }
    }
}
