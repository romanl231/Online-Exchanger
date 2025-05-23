using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.DTOs.ListingDTOs
{
    public class SearchByTitleDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public PaginationDTO Pagination { get; set; }
    }
}
