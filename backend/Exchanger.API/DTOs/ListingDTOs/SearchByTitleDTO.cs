using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.DTOs.ListingDTOs
{
    public class SearchByTitleDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public PaginationDTO Pagination { get; set; } = new PaginationDTO();
    }
}
