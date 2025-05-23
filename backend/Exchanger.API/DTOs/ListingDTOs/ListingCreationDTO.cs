using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.DTOs.ListingDTOs
{
    public class ListingCreationDTO
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public List<IFormFile> Images { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public List<int> CategoryIds { get; set; }
    }
}
