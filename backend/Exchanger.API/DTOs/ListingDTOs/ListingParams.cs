using Exchanger.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.DTOs.ListingDTOs
{
    public class ListingParams
    {
        public decimal MaxValue { get; set; } = 999999999m;
        public decimal MinValue { get; set; } = 0m;
        public List<int> CategoryIds { get; set; } = new List<int>();

        [Required]
        public PaginationDTO Pagination { get; set; } = new PaginationDTO();
    }
}
