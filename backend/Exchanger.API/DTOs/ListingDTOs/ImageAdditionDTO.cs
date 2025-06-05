using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.DTOs.ListingDTOs
{
    public class ImageAdditionDTO
    {
        [Required]
        public Guid listingId { get; set; }

        [Required]
        public List<IFormFile> files { get; set; } = new List<IFormFile>();
    }
}
